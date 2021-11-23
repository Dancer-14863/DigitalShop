using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Basket.API.Data;
using Basket.API.Filters;
using Basket.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Repositories
{
    public class BasketProductRepository : GenericRepository<BasketProduct>
    {
        public BasketProductRepository(BasketContext context) : base(context)
        {
        }

        public async Task<IEnumerable<BasketProduct>> GetByBasketId(int basketId)
        {
            return await _context.BasketProducts.Where(x => x.BasketId == basketId).ToListAsync();
        }
    }
}