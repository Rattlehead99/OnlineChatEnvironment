
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using OnlineChatEnvironment.Data.Models;
using OnlineChatEnvironment.Data;

namespace OnlineChatEnvironment.Hubs
{
    public class ChatHub : Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }

        public Task JoinRoom(string roomName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }


        public Task LeaveRoom(string roomName)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        //TO Uncomment when doing real-time
        //public async Task SendMessage(Guid roomId,
        //    string message,
        //    [FromServices] ApplicationDbContext db)
        //{
        //    var messageToSend = new Message
        //    {
        //        ChatId = roomId,
        //        Text = message,
        //        Name = Context.UserIdentifier,
        //        Timestamp = DateTime.UtcNow
        //    };

        //    db.Messages.Add(messageToSend);
        //    await db.SaveChangesAsync();

        //    await Clients.Group(roomId.ToString()).SendAsync("RecieveMessage", messageToSend); 
        //}

    }
}
