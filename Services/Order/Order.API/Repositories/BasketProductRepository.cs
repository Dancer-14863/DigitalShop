using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.Models;

namespace Order.API.Repositories
{
    public class BasketProductRepository : GenericRepository<BasketProduct>
    {
        public BasketProductRepository(OrderContext context) : base(context)
        {
        }

        public async Task<IEnumerable<BasketProduct>> GetByBasketId(int basketId)
        {
            return await _context.BasketProducts.Where(x => x.BasketId == basketId).ToListAsync();
        }
    }
}