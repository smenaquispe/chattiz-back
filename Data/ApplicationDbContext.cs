using Microsoft.EntityFrameworkCore;
using chattiz_back.Models;

namespace chattiz_back.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<UserModel> Users { get; set; }

    public DbSet<ChatModel> Chats { get; set; }

    public DbSet<MessageModel> Messages { get; set; }

}