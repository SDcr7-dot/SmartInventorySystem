namespace SmartInventorySystem.DTOs
{
    public class DashboardDto
    {
        public int TotalProducts { get; set; }

        public decimal TotalInventoryValue { get; set; }

        public int LowStockProducts { get; set; }
    }
}
