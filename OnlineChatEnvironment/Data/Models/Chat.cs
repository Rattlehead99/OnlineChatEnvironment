using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace OnlineChatEnvironment.Data.Models
{
    public class Chat
    {
        [Key]
        public Guid Id { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [Required]
        public ChatType Type { get; set; }

       
        public ICollection<Message> Messages { get; set; } = new List<Message>();

        public ICollection<ChatUser> Users { get; set; } = new List<ChatUser>();

       

    }
}
