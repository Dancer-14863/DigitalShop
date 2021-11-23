using Catalog.API.Repositories;
using Order.API.Data;

namespace Order.API.Repositories
{
    public class BasketRepository : GenericRepository<Models.Basket>
    {
        public BasketRepository(OrderContext context) : base(context)
        {
        }
    }
}