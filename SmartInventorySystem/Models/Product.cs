namespace SmartInventorySystem.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public int ReorderLevel { get; set; }

        public Category? Category { get; set; }

        public bool IsDeleted { get; set; } = false;

        public string? ImageUrl { get; set; }
    }
}