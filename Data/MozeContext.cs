namespace MozeApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MozeApi.Entities;

public class MozeContext(DbContextOptions<MozeContext> options) : IdentityDbContext<User, Role, string>(options)
{
    //Tables
    public DbSet<User> User { get; set; } = null!;
    public DbSet<Role> Role { get; set; } = null!;
    public DbSet<Transaction> Transaction { get; set; } = null!;
    public DbSet<Balance> Balance { get; set; } = null!;
    public DbSet<AppUrl> AppUrl { get; set; } = null!;

    // public DbSet<Log> Log { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        //Identity
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");

        // Entity Relationships
        // Transaction -> User (N:1)
        builder.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // AppUrl -> Transaction (1:1)
        builder.Entity<AppUrl>()
            .HasOne(a => a.Transaction)
            .WithOne()
            .HasForeignKey<AppUrl>(a => a.TransactionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}