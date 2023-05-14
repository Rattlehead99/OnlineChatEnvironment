namespace OnlineChatEnvironment.Data.Models
{
    public class Chat
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ChatType Type { get; set; }

       
        public ICollection<Message> Messages { get; set; }

        public ICollection<User> Users { get; set; }

       

    }
}
