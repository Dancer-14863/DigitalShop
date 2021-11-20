using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using User.API.Authorization;
using User.API.Models;

namespace User.API.Policies
{
    public class AccountAuthorizationHandler : AuthorizationHandler<AccountOwnerRequirement, Account>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccountOwnerRequirement requirement, Account resource)
        {
            if (context.User.IsInRole("Admin") || context.User.HasClaim(ClaimTypes.NameIdentifier, resource.Id.ToString()))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}