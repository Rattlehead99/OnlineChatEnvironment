using OnlineChatEnvironment.Data.Models;

namespace OnlineChatEnvironment.Infrastructure.Services
{
    public interface IChatService
    {
        Chat GetChat(Guid id);

        IEnumerable<Chat> GetChats(Guid userId);
        IEnumerable<Chat> GetPrivateChats(Guid userId);

        Task<Chat> CreatePrivateRoom(Guid rootId, Guid targetId);

        Task CreateRoom(string roomName, Guid roomId);

        Task JoinRoom(Guid chatId, Guid userId);

        Task<Message> CreateMessage(Guid chatId, Guid userId, string message);
        
    }
}
