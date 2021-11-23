using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Order.API.Authorization;

namespace Order.API.Policies
{
    public class OrderAuthorizationHandler : AuthorizationHandler<OrderOwnerRequirement, Models.Order>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OrderOwnerRequirement requirement, Models.Order resource)
        {
            if (context.User.IsInRole("Admin") || context.User.HasClaim(ClaimTypes.NameIdentifier, resource.OwnerId.ToString()))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}