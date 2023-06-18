using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineChatEnvironment.Data;
using OnlineChatEnvironment.Data.Models;
using OnlineChatEnvironment.Infrastructure;
using OnlineChatEnvironment.Infrastructure.Services;
using OnlineChatEnvironment.Models;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using static OnlineChatEnvironment.Data.DataConstants;

namespace OnlineChatEnvironment.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IChatService service;

        public HomeController(ILogger<HomeController> logger, IChatService service)
        {
            _logger = logger;
            this.service = service;
        }

        public IActionResult Index()
        {
            var chats = service.GetChats(GetUserId());

            return View(chats);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            await service.CreateRoom(name, GetUserId());

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> JoinRoom(Guid chatId)
        {
            await service.JoinRoom(chatId, GetUserId());

            return RedirectToAction("Chat", "Home", new { id = chatId });
        }

        public IActionResult Find([FromServices] ApplicationDbContext db)
        {
            var users = db.Users
                .Where(x => x.Id != GetUserId())
                .ToList();

            return View(users);
        }

        public IActionResult Private()
        {
            var chats = service.GetPrivateChats(GetUserId());

            return View(chats);
        }

        #region
        //private List<Chat> GetPrivateRooms()
        //{
        //    var chats = db.Chats
        //                  .Include(x => x.Users)
        //                  .ThenInclude(x => x.User)
        //                  .Where(x => x.Type == ChatType.Private
        //                      && x.Users.Any(y => y.UserId == GetUserId()))
        //                  .ToList();
        //    return chats;
        //}
        #endregion

        public async Task<IActionResult> CreatePrivateRoom(Guid userId)
        {
            if (service.GetPrivateRooms(userId).FirstOrDefault(x => x.Users.Any(y => y.UserId == userId)) is Chat exists)
            {
                return RedirectToAction("Chat", new { id = exists.Id }); ;
            }

            var chat = await service.CreatePrivateRoom(GetUserId(), userId);

            return RedirectToAction("Chat", new { id = chat.Id });
        }

        [HttpGet("[action]/{id}")]
        public IActionResult Chat(Guid id)
        {

            var chat = service.GetChat(id);

            return View(chat);
        }

        [HttpPost]
        public async Task<IActionResult> JoinChat(Guid chatId)
        {
            await service.JoinChat(chatId, GetUserId());

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(Guid roomId, string message)
        {
            await service.CreateMessage(roomId, User.Identity.Name, message);

            return RedirectToAction("Chat", new { id = roomId });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}