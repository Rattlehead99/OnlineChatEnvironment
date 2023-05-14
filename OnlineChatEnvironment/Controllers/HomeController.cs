using Microsoft.AspNetCore.Mvc;
using OnlineChatEnvironment.Data;
using OnlineChatEnvironment.Data.Models;
using OnlineChatEnvironment.Models;
using System.Diagnostics;

namespace OnlineChatEnvironment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(string name)
        {
            db.Chats.Add(new Chat
            {
                Name = name,
                Type = ChatType.Room,
            });

            await db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}