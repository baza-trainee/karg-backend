using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

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
