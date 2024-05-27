using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Interfaces
{
    public interface IFAQRepository
    {
        Task<List<FAQ>> GetFAQs();
        Task<int> AddFAQ(FAQ faq);
    }
}
