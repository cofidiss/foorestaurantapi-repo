using Microsoft.AspNetCore.Authorization;

namespace FooRestaurantApi.APILayer.Authorization
{
    public class RoleIdRequirement : IAuthorizationRequirement
    {

        public long RoleId { get; }
        public RoleIdRequirement(long roleId)
        {
            RoleId = roleId;
        }

    }
}
