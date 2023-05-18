
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace OnlineChatEnvironment.Hubs
{
    public class ChatHub : Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
       
    }
}
