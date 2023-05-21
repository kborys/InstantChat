using InstantChat.SignalR.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InstantChat.SignalR.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Message>()
            .HasOne<AppUser>(a => a.User)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.UserId);
    }

    private DbSet<Message> Messages { get; set; }
}