using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using User.API.Data;
using User.API.Models;

namespace User.API.Repositories
{
    public class AccountRepository : GenericRepository<Account>
    {
        public AccountRepository(UserContext context) : base(context)
        {
        }
        
        public async Task<Account?> GetAccountByEmail(string email)
        {
            return await _table.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}