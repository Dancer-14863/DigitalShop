using System.Security.Claims;
using System.Threading.Tasks;
using Catalog.API.Authorization;
using Catalog.API.Models;
using Microsoft.AspNetCore.Authorization;

namespace Catalog.API.Policies
{
    public class ProductAuthorizationHandler : AuthorizationHandler<ProductOwnerRequirement, Product>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProductOwnerRequirement requirement, Product resource)
        {
            if (context.User.IsInRole("Admin") || context.User.HasClaim(ClaimTypes.NameIdentifier, resource.OwnerId.ToString()))
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}