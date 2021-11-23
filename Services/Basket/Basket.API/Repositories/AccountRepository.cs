using Basket.API.Data;
using Basket.API.Models;

namespace Basket.API.Repositories
{
    public class AccountRepository : GenericRepository<Account>
    {
        public AccountRepository(BasketContext context) : base(context)
        {
        }
    }
}