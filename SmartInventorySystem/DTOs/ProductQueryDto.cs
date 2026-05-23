namespace SmartInventorySystem.DTOs
{
    public class ProductQueryDto
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 5;

        public string? Search { get; set; }

        public string? SortBy { get; set; }

        public int? CategoryId { get; set; }
    }
}