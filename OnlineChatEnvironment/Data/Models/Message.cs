﻿using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineChatEnvironment.Data.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }

        [ForeignKey(nameof(Chat))]
        public Guid ChatId { get; set; }
        public Chat Chat{ get; set; }
    }
}
