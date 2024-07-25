using karg.DAL.Models;

namespace karg.DAL.Interfaces
{
    public interface IRescuerRepository : IBaseRepository<Rescuer>
    {
        Task<Rescuer> GetRescuerByEmail(string email);
    }
}