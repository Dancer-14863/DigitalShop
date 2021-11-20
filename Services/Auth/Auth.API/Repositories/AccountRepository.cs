using System.Threading.Tasks;
using Auth.API.Data;
using Auth.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Repositories
{
    public class AccountRepository : GenericRepository<Account>
    {
        public AccountRepository(AuthContext context) : base(context)
        {
        }
        
        public async Task<Account> GetAccountByEmail(string email)
        {
            return await _table.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}