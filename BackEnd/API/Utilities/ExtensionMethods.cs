using System.Security.Claims;
using ZstdSharp;

namespace BackEnd.Utilities
{
    public static class ExtensionMethods
    {
        public static string GetUserId(this ClaimsPrincipal principle)
        {
            if (principle == null)
            {
                throw new ArgumentNullException(nameof(principle));
            }

            var claim = principle.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null || claim.Value == null) throw new Exception("User Name Identifier claim or value is null");

            return claim.Value;
        }
    }
}
