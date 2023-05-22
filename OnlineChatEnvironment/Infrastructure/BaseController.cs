using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OnlineChatEnvironment.Infrastructure
{
    public class BaseController : Controller
    {
        protected Guid GetUserId()
        {
            return Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
