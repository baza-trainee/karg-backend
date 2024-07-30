using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.BLL.Interfaces
{
    public interface IPaginationService<T>
    {
        Task<(List<T> Items, int TotalPages)> PaginateWithTotalPages(List<T> items, int page, int pageSize);
    }
}
