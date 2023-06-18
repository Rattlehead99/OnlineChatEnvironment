using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineChatEnvironment.Data;
using OnlineChatEnvironment.Data.Models;
using System.Security.Claims;
using System.Xml.Linq;

namespace OnlineChatEnvironment.Infrastructure.Services
{
    public class ChatService : IChatService
    {
        private ApplicationDbContext db;

        public ChatService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<OnlineChatEnvironment.Data.Models.Message> CreateMessage(Guid chatId, string userName, string message)
        {
            var messageToSend = new OnlineChatEnvironment.Data.Models.Message
            {
                ChatId = chatId,
                Text = message,
                Name = userName,
                Timestamp = DateTime.UtcNow
            };

            db.Messages.Add(messageToSend);
            await db.SaveChangesAsync();

            return messageToSend;
        }

        public async Task<Chat> CreatePrivateRoom(Guid rootId, Guid targetId)
        {
            var chat = new Chat
            {
                Type = ChatType.Private,
                Name = rootId.ToString()
                
            };

            chat.Users.Add(new ChatUser
            {
                UserId = targetId,

            });

            chat.Users.Add(new ChatUser
            {
                UserId = rootId

            });

            db.Chats.Add(chat);

            await db.SaveChangesAsync();

            return chat;
        }

        public async Task CreateRoom(string roomName, Guid userId)
        {
            var chat = new Chat
            {
                Name = roomName,
                Type = ChatType.Room,
            };

            chat.Users.Add(new ChatUser
            {

                UserId = userId,
                Role = UserRole.Admin
            });

            db.Chats.Add(chat);

            await db.SaveChangesAsync();
        }

        public Chat GetChat(Guid id)
        {
                return db.Chats
                .Include(x => x.Messages)
                .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Chat> GetChats(Guid userId)
        {
            var chats = db.Chats
                .Include(x => x.Users)
                .Where(x => !x.Users.Any(y => y.UserId == userId) && x.Type == 0)
                .ToList();
           
            return chats;
        }

        public IEnumerable<Chat> GetPrivateChats(Guid userId)
        {
           return db.Chats
                .Include(x => x.Users)
                .ThenInclude(x => x.User)
                .Where(x => x.Type == ChatType.Private
                    && x.Users.Any(y => y.UserId == userId))
                .ToList();
        }

        public async Task JoinRoom(Guid chatId, Guid userId)
        {
            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = userId,
                Role = UserRole.Member
            };

            db.ChatUsers.Add(chatUser);

            await db.SaveChangesAsync();
        }

        public async Task JoinChat(Guid chatId, Guid userId)
        {
            var chatUser = new ChatUser
            {
                ChatId = chatId,
                UserId = userId,
                Role = UserRole.Member

            };

            db.ChatUsers.Add(chatUser);

            await db.SaveChangesAsync();
        }

        public List<Chat> GetPrivateRooms(Guid userId)
        {
            var chats = db.Chats
                          .Include(x => x.Users)
                          .ThenInclude(x => x.User)
                          .Where(x => x.Type == ChatType.Private
                              && x.Users.Any(y => y.UserId == userId))
                          .ToList();
            return chats;
        }

    }
}
