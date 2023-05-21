using System.Diagnostics.CodeAnalysis;

namespace OnlineChatEnvironment.Data.Models
{
    public class Chat
    {
        public Chat()
        {
        }
        public Guid Id { get; set; } = Guid.NewGuid();//Might need to remove Guid.NewGuid()

        
        public string Name { get; set; }

        public ChatType Type { get; set; }

       
        public ICollection<Message> Messages { get; set; } = new List<Message>();

        public ICollection<ChatUser> Users { get; set; } = new List<ChatUser>();

       

    }
}
