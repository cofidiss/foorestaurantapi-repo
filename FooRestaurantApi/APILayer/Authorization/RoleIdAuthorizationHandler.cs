using FooRestaurantApi.DomainLayer.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FooRestaurantApi.APILayer.Authorization
{
    public class RoleIdAuthorizationHandler : AuthorizationHandler<RoleIdRequirement>
    {
        private readonly ILogger<RoleIdAuthorizationHandler> _logger;
        public RoleIdAuthorizationHandler()
        {

        }


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleIdRequirement requirement)
        {
            long roleId = -1;
            if (!Int64.TryParse(context.User.FindFirst(c => c.Type == nameof(UserEntity.RoleId))?.Value, out roleId))
            {
                return Task.CompletedTask;


            }
            if (roleId == requirement.RoleId)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
