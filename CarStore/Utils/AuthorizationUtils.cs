using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CarStore.Utils
{
    public static class AuthorizationUtils
    {
        public static bool IsAdmin(HttpContext context)
        {
            var role = context.User.FindFirst("Role")?.Value;
            return role == "Admin";
        }
    }
}
