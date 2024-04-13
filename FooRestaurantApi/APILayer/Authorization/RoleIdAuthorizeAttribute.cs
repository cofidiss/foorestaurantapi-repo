using Microsoft.AspNetCore.Authorization;

namespace FooRestaurantApi.APILayer.Authorization
{
    public class RoleIdAuthorizeAttribute : AuthorizeAttribute
    {
        public const string POLICY_PREFIX = "RoleId";
        public RoleIdAuthorizeAttribute()
        {

        }
        public RoleIdAuthorizeAttribute(int roleId)
        {
            Policy = POLICY_PREFIX + roleId;

        }
    }
}
