namespace karg.BLL.Interfaces.Utilities
{
    public interface IPaginationService<T>
    {
        Task<(List<T> Items, int TotalPages)> PaginateWithTotalPages(List<T> items, int page, int pageSize);
    }
}
