using User.API.Data;
using User.API.Models;

namespace User.API.Repositories
{
    public class RoleRepository : GenericRepository<Role>
    {
        public RoleRepository(UserContext context) : base(context)
        {
        }
    }
}