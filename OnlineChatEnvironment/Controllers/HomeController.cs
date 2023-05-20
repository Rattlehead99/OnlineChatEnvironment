using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineChatEnvironment.Data;
using OnlineChatEnvironment.Data.Models;
using OnlineChatEnvironment.Infrastructure;
using OnlineChatEnvironment.Infrastructure.Services;
using OnlineChatEnvironment.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace OnlineChatEnvironment.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IChatService chatService;

        public HomeController(ILogger<HomeController> logger, IChatService service)
        {
            _logger = logger;
            chatService = service;
        }

        public IActionResult Index()
        {
            var chats = chatService.GetChats(GetUserId());

            return View(chats);
        }

        public async Task<IActionResult> CreatePrivateRoom(Guid userId)
        {
            var chat = await chatService.CreatePrivateRoom(GetUserId(), userId);

            return View("Chat", chat);
            
        }

        public IActionResult Find([FromServices] ApplicationDbContext db)
        {
            var users = db.Users
                          .Where(x => x.Id != User.GetUserId())
                          .ToList();

            return View(users);
        }

        public IActionResult Private()
        {
            var chats = chatService.GetPrivateChats(GetUserId());

            return View(chats);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            await chatService.CreateRoom(name,GetUserId());

            return RedirectToAction("Index");
        }

        [HttpGet("{id}")]
        public IActionResult Chat(Guid id)
        {
            var chat = chatService.GetChat(id);

            return View(chat);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(Guid chatId, string message)
        {

            var messageText = new Message
            {
                ChatId = chatId,
                Text = message,
                Name = User.Identity.Name,
                Timestamp = DateTime.UtcNow
            };

            //db.Messages.Add(messageText);
            //await db.SaveChangesAsync();

            return RedirectToAction("Chat", new { id = chatId});
        }

        [HttpPost]
        public async Task<IActionResult> JoinChat(Guid chatId)
        {
            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = User.GetUserId(),
                Role = UserRole.Member

            };

            //db.ChatUsers.Add(chatUser);

           // await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> JoinRoom(Guid chatId)
        {
            await chatService.JoinRoom(chatId, GetUserId());

            return RedirectToAction("Chat", "Home", new {id = chatId});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}