
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace OnlineChatEnvironment.Hubs
{
    public class ChatHub : Hub
    {
        public string GetConnctionid()
        {
            return Context.ConnectionId;
        }
       
    }
}
