using System.ComponentModel.DataAnnotations;

namespace SmartInventorySystem.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        [Range(1, 1000000)]
        public decimal Price { get; set; }

        [Range(0, 10000)]
        public int Quantity { get; set; }

        [Range(1, 1000)]
        public int ReorderLevel { get; set; }
    }
}