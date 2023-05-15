using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineChatEnvironment.Data.Models;

namespace OnlineChatEnvironment.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChatUser>()
                .HasKey(x => new { x.ChatId, x.UserId });
        }

    }
}