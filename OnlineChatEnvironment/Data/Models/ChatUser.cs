using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineChatEnvironment.Data.Models
{
    public class ChatUser
    {

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey(nameof(Chat))]
        public Guid ChatId { get; set; }
        public Chat Chat { get; set; }

        public UserRole Role { get; set; }
    }
}
