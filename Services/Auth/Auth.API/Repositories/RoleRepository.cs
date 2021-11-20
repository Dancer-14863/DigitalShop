using System.Threading.Tasks;
using Auth.API.Data;
using Auth.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Repositories
{
    public class RoleRepository : GenericRepository<Role>
    {
        public RoleRepository(AuthContext context) : base(context)
        {
        }
        
        // get role by name
        public Task<Role?> GetRoleByName(string name)
        {
            return _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }
    }
}