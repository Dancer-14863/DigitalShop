using Microsoft.AspNetCore.Authorization;

namespace Order.API.Authorization
{
    public class OrderOwnerRequirement : IAuthorizationRequirement
    {
    }
}