using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineChatEnvironment.Data.Models
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [ForeignKey(nameof(Chat))]
        public Guid ChatId { get; set; }
        public Chat Chat{ get; set; }
    }
}
