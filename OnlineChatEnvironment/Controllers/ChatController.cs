using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using OnlineChatEnvironment.Hubs;
using Microsoft.AspNetCore.Authorization;
using OnlineChatEnvironment.Data;
using OnlineChatEnvironment.Data.Models;

namespace OnlineChatEnvironment.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private IHubContext<ChatHub> chat;

        public ChatController(IHubContext<ChatHub> chat)
        {
            this.chat = chat;
        }

        [HttpPost("[action]/{connectionId}/{roomName}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomName)
        {
            
            await chat.Groups.AddToGroupAsync(connectionId, roomName);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string roomName)
        {
            await chat.Groups.RemoveFromGroupAsync(connectionId, roomName);

            return Ok();
        }

        public async Task<IActionResult> SendMessage(Guid chatId, string message,string roomName, [FromServices] ApplicationDbContext db)
        {
            var messageToSend = new Message
            {
                ChatId = chatId,
                Text = message,
                Name = User.Identity.Name,
                Timestamp = DateTime.UtcNow
            };

            db.Messages.Add(messageToSend);
            await db.SaveChangesAsync();

            //await chat.Clients.Group(roomName).SendAsync("RecieveMessage", messageToSend);
            await chat.Clients.Group(roomName).SendAsync("RecieveMessage", new
            {
                Text = messageToSend.Text,
                Name =messageToSend.Name,
                TimeStamp = messageToSend.Timestamp.ToString("dd/MM/yyyy hh:mm:ss")
            });
            
        
            return Ok();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
