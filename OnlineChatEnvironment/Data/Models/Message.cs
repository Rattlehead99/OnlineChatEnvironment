namespace OnlineChatEnvironment.Data.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public DateTime Timestamps { get; set; }
    }
}
