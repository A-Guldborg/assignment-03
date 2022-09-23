namespace Assignment3.Entities;

public class KanbanContext : DbContext
{
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Task>()
            .Property(s => s.State)
            .HasConversion(
                v => v.ToString(),
                v => (State)Enum.Parse(typeof(State), v));
    }
}
