using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace FooRestaurantApi.APILayer.Authorization
{
    public class RoleIddPolicyProvider : IAuthorizationPolicyProvider
    {
        public const string POLICY_PREFIX = RoleIdAuthorizeAttribute.POLICY_PREFIX;
        private DefaultAuthorizationPolicyProvider BackupPolicyProvider { get; }
        public RoleIddPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            BackupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }
        public async Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return await BackupPolicyProvider.GetDefaultPolicyAsync();
        }

        public async Task<AuthorizationPolicy> GetFallbackPolicyAsync()
        {
            return await BackupPolicyProvider.GetFallbackPolicyAsync();
        }

        public async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase))

            {
                var roleId = Convert.ToInt64(policyName.Substring(POLICY_PREFIX.Length));
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new RoleIdRequirement(roleId));
                return policy.Build();
            }

            return await BackupPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
