using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    /**
     * A generic repository interface should be implemented by all repositories.
     */
    public interface IGenericRepository<T> where T : class
    {
        Task<int> Count();
        Task<T?> Get(int id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Add(T entity);
        Task<T?> Update(T entity);
        Task<T?> Delete(T entity);
    }
}