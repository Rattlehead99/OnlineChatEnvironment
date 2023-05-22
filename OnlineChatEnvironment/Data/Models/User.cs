using Microsoft.AspNetCore.Identity;
using static OnlineChatEnvironment.Data.DataConstants;

namespace OnlineChatEnvironment.Data.Models
{
    public class User : IdentityUser<Guid>
    {
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        public ICollection<Article> Articles { get; set; } = new List<Article>();

        public ICollection<ChatUser> Chats { get; set; } = new List<ChatUser>();
    }
}
