using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Data;
using Basket.API.Filters;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Repositories
{
    public class BasketRepository : GenericRepository<Models.Basket>
    {
        public BasketRepository(BasketContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Models.Basket?>> GetAllFilteredPaginated(int page, int pageSize,
            BasketColumnFilter basketColumnFilter)
        {
            IQueryable<Models.Basket> baskets = _table;
            baskets = baskets.Where(a =>
                (basketColumnFilter.OwnerId == 0 || a.OwnerId == basketColumnFilter.OwnerId));

            return await baskets
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}