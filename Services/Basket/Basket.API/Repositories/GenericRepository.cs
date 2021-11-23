using System.Collections.Generic;
using System.Threading.Tasks;
using Basket.API.Data;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly BasketContext _context;
        protected readonly DbSet<T> _table;

        public GenericRepository(BasketContext context)
        {
            _context = context;
            _table = _context.Set<T>();
        }
        
        public async Task<int> Count()
        {
            return await _table.CountAsync();
        }

        public async Task<T?> Get(int id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _table.ToListAsync();
        }

        public async Task<T> Add(T entity)
        {
            _table.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> Update(T entity)
        {
            _table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> Delete(T entity)
        {
            _table.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}