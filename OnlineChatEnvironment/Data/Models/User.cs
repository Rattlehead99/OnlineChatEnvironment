using Microsoft.AspNetCore.Identity;

namespace OnlineChatEnvironment.Data.Models
{
    public class User : IdentityUser<Guid>
    {
        
        public ICollection<ChatUser> Chats { get; set; } = new List<ChatUser>();
    }
}
