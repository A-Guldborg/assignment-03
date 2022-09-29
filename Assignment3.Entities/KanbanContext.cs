using System.Collections.Immutable;
using Assignment3.Core;

namespace Assignment3.Entities;

public class KanbanContext : DbContext
{
    public KanbanContext(DbContextOptions<KanbanContext> options) : base(options){}

    public virtual DbSet<Task> Tasks => Set<Task>();
    public virtual DbSet<User> Users => Set<User>();
    public virtual DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Task>()
            .Property(s => s.State)
            .HasConversion(
                v => v.ToString(),
                v => (State)Enum.Parse(typeof(State), v));

        modelBuilder.Entity<Task>().Property(t => t.Title).HasMaxLength(100);
        modelBuilder.Entity<Task>().HasMany(s => s.Tags).WithMany(c => c.Tasks);
        
        modelBuilder.Entity<User>().Property(u => u.Name).HasMaxLength(100);
        modelBuilder.Entity<User>().Property(u => u.Email).HasMaxLength(100);
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        
        modelBuilder.Entity<Tag>().HasIndex(t => t.Name).IsUnique();
        modelBuilder.Entity<Tag>().Property(t => t.Name).HasMaxLength(50);
        modelBuilder.Entity<Tag>().HasMany(s => s.Tasks).WithMany(c => c.Tags);
    }
}
