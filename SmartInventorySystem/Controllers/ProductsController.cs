using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartInventorySystem.Data;
using SmartInventorySystem.DTOs;
using SmartInventorySystem.Interfaces;
using SmartInventorySystem.Models;
using SmartInventorySystem.Responses;

namespace SmartInventorySystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly AppDbContext _context;
        private readonly ILogger<ProductsController> _logger;
        private object id;

        public ProductsController(IProductService service, AppDbContext context, ILogger<ProductsController> logger)
        {
            _service = service;
            _context = context;
            _logger = logger;
        }
        [HttpGet("advanced")]
        public async Task<IActionResult> GetAdvancedProducts(
    [FromQuery] ProductQueryDto query)
        {
            var result =
                await _service.GetProductsAdvanced(query);

            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Fetching all products");
            var data = await _service.GetProducts();

            return Ok(new ApiResponse<List<ProductDto>>
            {
                Success = true,
                Message = "Products fetched successfully",
                Data = data
            });
        }
        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockProducts()
        {
            var data = await _service.GetLowStockProducts();

            // Map List<Product> to List<LowStockProductDto> if necessary
            var dtoList = data.Select(p => new LowStockProductDto
            {
                Name = p.Name,
                Quantity = p.Quantity,
                ReorderLevel = p.ReorderLevel
            }).ToList();

            return Ok(new ApiResponse<List<LowStockProductDto>>
            {
                Success = true,
                Message = "Low stock products fetched successfully",
                Data = dtoList
            });
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search(string keyword)
        {
            var data = await _service.SearchProducts(keyword);

            return Ok(new ApiResponse<List<ProductDto>>
            {
                Success = true,
                Message = "Products fetched successfully",
                Data = data
            });
        }
        [HttpGet("sorted-by-price")]
        public async Task<IActionResult> GetSortedProducts()
        {
            var data = await _service.GetProductsSortedByPriceDesc();

            return Ok(new ApiResponse<List<ProductDto>>
            {
                Success = true,
                Message = "Products sorted by price fetched successfully",
                Data = data
            });
        }
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedProducts(
     int pageNumber = 1,
     int pageSize = 5)
        {
            var allProducts = await _service.GetProducts();

            var pagedData = allProducts
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var response = new PagedResponse<ProductDto>
            {
                Success = true,
                Message = "Paged products fetched successfully",

                Data = pagedData,

                PageNumber = pageNumber,

                PageSize = pageSize,

                TotalRecords = allProducts.Count,

                TotalPages =
                    (int)Math.Ceiling(
                        (double)allProducts.Count / pageSize)
            };

            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetProductById(id);

            return Ok(new ApiResponse<ProductDto>
            {
                Success = true,
                Message = "Product fetched successfully",
                Data = data
            });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(CreateProductDto dto)
        {
            _logger.LogInformation(
             $"Adding new product: {dto.Name}");
            var result = await _service.AddProduct(dto);

            return Ok(new ApiResponse<ProductDto>
            {
                Success = true,
                Message = "Product added successfully",
                Data = result
            });
        }
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Uploads");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new
            {
                Message = "File uploaded successfully",
                FileName = file.FileName
            });
        }
        [HttpGet("export")]
        public async Task<IActionResult> ExportProducts()
        {
            // Fix: Cast _context.Products to the correct DbSet<Product> type
            var products = await _context.Products.ToListAsync();

            using var workbook = new ClosedXML.Excel.XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Products");

            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Price";
            worksheet.Cell(1, 4).Value = "Quantity";

            for (int i = 0; i < products.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = products[i].Id;
                worksheet.Cell(i + 2, 2).Value = products[i].Name;
                worksheet.Cell(i + 2, 3).Value = products[i].Price;
                worksheet.Cell(i + 2, 4).Value = products[i].Quantity;
            }

            using var stream = new MemoryStream();

            workbook.SaveAs(stream);

            var content = stream.ToArray();

            return File(
                content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Products.xlsx");
        }
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            _logger.LogInformation(
               "Fetching dashboard data");
            _logger.LogWarning(
                $"Deleting product with ID {id}");
            var data = await _service.GetDashboardData();

            return Ok(new ApiResponse<DashboardDto>
            {
                Success = true,
                Message = "Dashboard fetched successfully",
                Data = data
            });
        }
        [HttpGet("category-summary")]
        public async Task<IActionResult> GetCategorySummary()
        {
            var data = await _service.GetCategorySummary();

            return Ok(new ApiResponse<List<CategorySummaryDto>>
            {
                Success = true,
                Message = "Category summary fetched successfully",
                Data = data
            });
        }

        [HttpPost("{id}/upload-image")]
        public async Task<IActionResult> UploadProductImage(
            int id,
            IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound("Product not found");

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "product-images");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName =
                Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var filePath = Path.Combine(
                uploadsFolder,
                uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            product.ImageUrl =
                $"/product-images/{uniqueFileName}";

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Image uploaded successfully",
                ImageUrl = product.ImageUrl
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProductDto dto)
        {
            var result =
                await _service.UpdateProduct(id, dto);

            return Ok(new ApiResponse<ProductDto>
            {
                Success = true,
                Message = "Product updated successfully",
                Data = result
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.SoftDeleteProduct(id);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Product deleted successfully",
                Data = null
            });
        }

        [HttpPut("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            await _service.RestoreProduct(id);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                Message = "Product restored successfully",
                Data = null
            });
        }
    }
}