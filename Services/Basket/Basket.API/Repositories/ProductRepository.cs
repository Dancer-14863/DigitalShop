using Basket.API.Data;
using Basket.API.Models;

namespace Basket.API.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository(BasketContext context) : base(context)
        {
        }
    }
}
