namespace karg.BLL.DTO.Utilities
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalPages { get; set; }
    }
}
