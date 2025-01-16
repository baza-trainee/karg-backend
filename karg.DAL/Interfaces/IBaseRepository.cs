namespace karg.DAL.Interfaces
{
    public interface IBaseRepository<T>
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<int> Add(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}