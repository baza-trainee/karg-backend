using karg.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace karg.DAL.Interfaces
{
    public interface IAdviceRepository
    {
        Task<List<Advice>> GetAdvices();
        Task<Advice> GetAdvice(int adviceId);
        Task DeleteAdvice(Advice advice);
    }
}
