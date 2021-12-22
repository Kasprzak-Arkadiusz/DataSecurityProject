using System.Linq;
using System.Security.Claims;

namespace UI.Utils
{
    public static class ClaimsExtensions
    {
        public static string GetCurrentUserName(this ClaimsIdentity claimsIdentity)
        {
            var claim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == "Name");

            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}