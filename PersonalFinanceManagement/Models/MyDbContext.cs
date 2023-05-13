using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PersonalFinanceManagement.Models;

public class MyDbContext : IdentityDbContext<User, AppRole, Guid>
{   

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users { get; set; }
    public DbSet<Spending> Spendings { get; set; }
    
    public DbSet<Income> Incomes { get; set; }
    
    public DbSet<Category> Categories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(b =>
        {
            b.Property(u => u.Id).HasDefaultValueSql("newsequentialid()");
        });

        modelBuilder.Entity<AppRole>(b =>
        {
            b.Property(u => u.Id).HasDefaultValueSql("newsequentialid()");
        });
        modelBuilder.Entity<Income>(b =>
        {
            b.Property(u => u.Id).HasDefaultValueSql("newsequentialid()");
        });
        modelBuilder.Entity<Spending>(b =>
        {
            b.Property(u => u.Id).HasDefaultValueSql("newsequentialid()");
        });       
        modelBuilder.Entity<Category>(b =>
        {
            b.Property(u => u.Id).HasDefaultValueSql("newsequentialid()");
        });
        modelBuilder.Entity<User>()
            .HasMany(u => u.Spendings)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Incomes)
            .WithOne(i => i.User)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()
            .HasMany(c => c.Incomes)
            .WithOne(i => i.Category)
            .HasForeignKey(i => i.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Category>()
            .HasMany(c => c.Spendings)
            .WithOne(s => s.Category)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }



    public override int SaveChanges()
    {
        var currentTime = DateTime.UtcNow;
    
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Property("CreatedAt") != null)
                    entry.Property("CreatedAt").CurrentValue = currentTime;
    
                if (entry.Property("UpdatedAt") != null)
                    entry.Property("UpdatedAt").CurrentValue = currentTime;
            }
            else if (entry.State == EntityState.Modified)
            {
                if (entry.Property("UpdatedAt") != null)
                    entry.Property("UpdatedAt").CurrentValue = currentTime;
            }
        }
    
        return base.SaveChanges();
    }
}