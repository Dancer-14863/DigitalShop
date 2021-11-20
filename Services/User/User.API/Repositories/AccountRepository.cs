using User.API.Data;
using User.API.Models;

namespace User.API.Repositories
{
    public class AccountRepository : GenericRepository<Account>
    {
        public AccountRepository(UserContext context) : base(context)
        {
        }
    }
}