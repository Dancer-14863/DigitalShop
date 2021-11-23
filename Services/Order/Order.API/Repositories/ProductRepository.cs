using Catalog.API.Repositories;
using Order.API.Data;
using Order.API.Models;

namespace Order.API.Repositories
{
    public class ProductRepository : GenericRepository<Product>
    {
        public ProductRepository(OrderContext context) : base(context)
        {
        }
        
    }
}