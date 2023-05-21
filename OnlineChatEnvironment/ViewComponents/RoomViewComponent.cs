using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineChatEnvironment.Data;
using OnlineChatEnvironment.Data.Models;
using System.Security.Claims;

namespace OnlineChatEnvironment.ViewComponents
{
    public class RoomViewComponent : ViewComponent
    {
        private ApplicationDbContext db;

        public RoomViewComponent(ApplicationDbContext context)
        {
            db = context;
        }

        public IViewComponentResult Invoke()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {

            }
            var userId = Guid.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var chats = db.ChatUsers
                .Include(x => x.Chat)
                .Where(x => x.UserId == userId && x.Chat.Type == ChatType.Room)
                .Select(chatUsers => chatUsers.Chat)
                .ToList();

            return View(chats);
        }
    }
}
