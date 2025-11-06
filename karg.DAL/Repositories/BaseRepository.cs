using karg.DAL.Context;
using karg.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace karg.DAL.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly KargDbContext _context;

        public BaseRepository(KargDbContext context)
        {
            _context = context;
        }

        public virtual async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public virtual async Task<T> GetById(int id)
        {
            return await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);
        }

        public virtual async Task<int> Add(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
            return (int)typeof(T).GetProperty("Id").GetValue(entity, null);
        }

        public virtual async Task Update(T entity)
        {
            var existingEntity = await _context.Set<T>().FindAsync(typeof(T).GetProperty("Id").GetValue(entity, null));
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
