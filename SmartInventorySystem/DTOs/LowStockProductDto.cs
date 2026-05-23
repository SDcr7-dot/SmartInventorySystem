namespace SmartInventorySystem.DTOs
{
    public class LowStockProductDto
    {
        public string Name { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public int ReorderLevel { get; set; }
    }
}
