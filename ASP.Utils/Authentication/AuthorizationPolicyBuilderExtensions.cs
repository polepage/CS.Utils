using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace ASP.Utils.Authentication
{
    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireRole<T>(this AuthorizationPolicyBuilder policy, params T[] roles)
        {
            return policy.RequireRole(roles.Select(r => r.ToString()));
        }
    }
}
