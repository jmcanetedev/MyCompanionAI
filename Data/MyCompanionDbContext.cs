using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyCompanionAI.Data.Models;

namespace MyCompanionAI.Data;

public partial class MyCompanionDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public MyCompanionDbContext(DbContextOptions<MyCompanionDbContext> options) : base(options)
    {
    }
    public DbSet<Conversation> Conversations { get; set; } = null!;
    public DbSet<Chat> Chats { get; set; } = null!;
    public MyCompanionDbContext()
    {
    }
    partial void OnModelBuilding(ModelBuilder builder);
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
