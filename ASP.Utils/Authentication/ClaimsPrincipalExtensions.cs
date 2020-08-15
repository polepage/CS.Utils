using System.Linq;
using System.Security.Claims;

namespace ASP.Utils.Authentication
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsInRole<T>(this ClaimsPrincipal user, params T[] roles)
        {
            return roles.Any(r => user.IsInRole(r.ToString()));
        }
    }
}
