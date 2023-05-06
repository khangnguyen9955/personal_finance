using Microsoft.EntityFrameworkCore;

namespace PersonalFinanceManagement.Models;

public class MyDbContext: DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
    {
    } 
    public DbSet<User> Users { get; set; }
    public DbSet<Spending> Spendings { get; set; }
    
    public DbSet<Income> Incomes { get; set; }
    
    public DbSet<Category> Categories { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    
        modelBuilder.Entity<User>()
            .HasMany(u => u.Spendings)
            .WithOne(s => s.User);
    
        modelBuilder.Entity<User>()
            .HasMany(u => u.Incomes)
            .WithOne(i => i.User);
    
        modelBuilder.Entity<Spending>()
            .HasOne(s => s.Category)
            .WithMany(c => c.Spendings)
            .HasForeignKey(s => s.CategoryId);
    
        modelBuilder.Entity<Income>()
            .HasOne(i => i.Category)
            .WithMany(c => c.Incomes)
            .HasForeignKey(i => i.CategoryId);
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