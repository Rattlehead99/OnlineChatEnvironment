using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;
using OnlineChatEnvironment.Hubs;
using Microsoft.AspNetCore.Authorization;
using OnlineChatEnvironment.Data;
using OnlineChatEnvironment.Data.Models;
using OnlineChatEnvironment.Infrastructure.Services;
using Microsoft.Owin.Security.Provider;
using OnlineChatEnvironment.Infrastructure;
using System.Security.Claims;

namespace OnlineChatEnvironment.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatController : BaseController
    {
        private IHubContext<ChatHub> chat;

        public ChatController(IHubContext<ChatHub> chat)
        {
            this.chat = chat;
        }

        [HttpPost("[action]/{connectionId}/{roomId}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomId)
        {
            
            await chat.Groups.AddToGroupAsync(connectionId, roomId);

            return Ok();
        }

        [HttpPost("[action]/{connectionId}/{roomId}")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string roomName)
        {
            await chat.Groups.RemoveFromGroupAsync(connectionId, roomName);

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(Guid roomId, string message, [FromServices] ApplicationDbContext db)
        {
            //var currentUserId = User.Identity.Name;
            //var userToSend = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            //var messageToSend = await db.CreateMessage(roomId, userToSend, message);

            var messageToSend = new Message
            {
                ChatId = roomId,
                Text = message,
                Name = User.Identity.Name,
                Timestamp = DateTime.UtcNow
            };

            db.Messages.Add(messageToSend);
            await db.SaveChangesAsync();

            await chat.Clients.Group(roomId.ToString()).SendAsync("RecieveMessage", messageToSend);
            //await chat.Clients.Group(roomId.ToString()).SendAsync("RecieveMessage", new
            //{
            //    Text = messageToSend.Text,
            //    Name =messageToSend.Name,
            //    TimeStamp = messageToSend.Timestamp.ToString("dd/MM/yyyy hh:mm:ss")
            //});

            return Ok();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
