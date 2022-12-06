using Demo_Role.CustomAuthorization.Handler;
using Demo_Role.CustomAuthorization.Requirement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Demo_Role.CustomAuthorization
{
        public class CustomAuthorizationPolicyProvider : IAuthorizationPolicyProvider
        {
            public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

            public CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
            {
                FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
            }

            public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
            {
                return FallbackPolicyProvider.GetDefaultPolicyAsync();
            }

            public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
            {
                var authPolicy = Task.FromResult<AuthorizationPolicy>(null);
                return authPolicy;
            }

            public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
            {
                if (policyName.Contains("."))
                {
                    string function = policyName.Split('.')[0];
                    string action = policyName.Split('.')[1];
                    var policy = new AuthorizationPolicyBuilder();
                    policy.AddRequirements(new CustomPermissionRequirement(action, function));
                    var authPolicy = Task.FromResult(policy.Build());
                    return authPolicy;
                }

                return FallbackPolicyProvider.GetPolicyAsync(policyName);
            }
        }
}
