using System.Security.Claims;
using System.Threading.Tasks;
using Basket.API.Authorization;
using Basket.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace Basket.API.Policies
{
    public class BasketAuthorizationHandler : AuthorizationHandler<BasketOwnerRequirement, Models.Basket>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, BasketOwnerRequirement requirement, Models.Basket resource)
        {
            if (context.User.IsInRole("Admin") || context.User.HasClaim(ClaimTypes.NameIdentifier, resource.OwnerId.ToString()))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}