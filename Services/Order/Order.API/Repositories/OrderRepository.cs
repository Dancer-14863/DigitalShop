using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Repositories;
using Microsoft.EntityFrameworkCore;
using Order.API.Data;
using Order.API.Filters;

namespace Order.API.Repositories
{
    public class OrderRepository : GenericRepository<Models.Order>
    {
        public OrderRepository(OrderContext context) : base(context)
        {
        }
        
        public async Task<IEnumerable<Models.Order?>> GetAllFilteredPaginated(int page, int pageSize,
            OrderColumnFilter orderColumnFilter)
        {
            IQueryable<Models.Order> orders = _table;
            if (orderColumnFilter.OwnerId != 0 ||  orderColumnFilter.IsPaid!= null)
            {
                orders = orders.Where(order =>
                    (orderColumnFilter.OwnerId == 0 || order.OwnerId == orderColumnFilter.OwnerId) &&
                    (orderColumnFilter.IsPaid == null || order.IsPaid == orderColumnFilter.IsPaid));
            }

            return await orders
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}