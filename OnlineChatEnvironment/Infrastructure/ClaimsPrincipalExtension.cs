using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace OnlineChatEnvironment.Infrastructure
{
    public static class ClaimsPrincipalExtension
    {
       public static Guid GetUserId(this ClaimsPrincipal @this)
        {
            return Guid.Parse(@this.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
