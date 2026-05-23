using Microsoft.EntityFrameworkCore;
using Quartz;
using SmartInventorySystem.Data;
using SmartInventorySystem.Interfaces;

namespace SmartInventorySystem.Jobs;

public class LowStockJob : IJob
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;

    public LowStockJob(
     AppDbContext context,
     IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine("JOB IS RUNNING");
        var lowStockProducts = await _context.Products
            .Where(p => !p.IsDeleted &&
                        p.Quantity <= p.ReorderLevel)
            .ToListAsync();

        Console.WriteLine("===== LOW STOCK CHECK STARTED =====");

        if (!lowStockProducts.Any())
        {
            Console.WriteLine("No low stock products found.");
        }

        foreach (var product in lowStockProducts)
        {
            Console.WriteLine(
                $"LOW STOCK ALERT: {product.Name} | Quantity: {product.Quantity} | Reorder Level: {product.ReorderLevel}");
            await _emailService.SendEmail(
    "shivamdeshmukh2002@gmail.com",
    "Low Stock Alert",
    $"Product {product.Name} is low on stock. Quantity: {product.Quantity}");
        }

        Console.WriteLine("===== LOW STOCK CHECK FINISHED =====");
    }
}