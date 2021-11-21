using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using User.API.Data;
using User.API.Filters;
using User.API.Models;

namespace User.API.Repositories
{
    public class AccountRepository : GenericRepository<Account>
    {
        public AccountRepository(UserContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Account?>> GetAllFilteredPaginated(int page, int pageSize,
            AccountColumnFilter accountColumnFilter)
        {
            IQueryable<Account> accounts = _table;
            if (accountColumnFilter.Name != null || accountColumnFilter.Email != null ||
                accountColumnFilter.RoleId != 0)
            {
                accounts = accounts.Where(a =>
                    (accountColumnFilter.Name == null || a.Name.Contains(accountColumnFilter.Name)) &&
                    (accountColumnFilter.Email == null || a.Email.Contains(accountColumnFilter.Email)) &&
                    (accountColumnFilter.RoleId == 0 || a.RoleId == accountColumnFilter.RoleId));
            }

            return await accounts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}