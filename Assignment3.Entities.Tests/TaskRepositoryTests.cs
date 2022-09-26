using System.Configuration;
using Assignment3.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Assignment3.Entities.Tests;

public class TaskRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly TaskRepository _repository;

    public TaskRepositoryTests()
    {
        var builder = new DbContextOptionsBuilder<KanbanContext>()
            .UseInMemoryDatabase("KanbanTest")
            .ConfigureWarnings(b => 
                b.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
        using var context = new KanbanContext(builder);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        // Repo specific adds
        var unassignedTask = new Task
        {
            Id = 1, Title = "Dishes", 
            Description = "There are a lot! Prepare.",
            State = State.New
        };

        var resolvedTask = new Task
        {
            Id = 2, Title = "Shopping",
            Description = "You're going on an adventure!",
            State = State.Resolved
        };
        
        
        var activeTask = new Task
        {
            Id = 3, Title = "Laundry", 
            Description = "Don't mix colors and white!",
            State = State.Active
        };
        
        var closedTask = new Task
        {
            Id = 4, Title = "Drink a beer", 
            Description = "It's an IPA!",
            State = State.Closed
        };
        
        var removedTask = new Task
        {
            Id = 5, Title = "Call mom", 
            Description = "Tell her you love her <3",
            State = State.Removed
        };
        
        context.AddRange(unassignedTask, resolvedTask, activeTask, closedTask, removedTask);

        context.SaveChanges();
        
        _context = context;
        _repository = new TaskRepository();
    }

    [Fact]
    public void Only_tasks_with_the_state_New_can_be_deleted_from_the_database()
    {
        var response = _repository.Delete(1);
        response.Should().Be(Response.Deleted);
    }
    
    [Fact]
    public void Deleting_a_task_which_is_Active_should_set_its_state_to_Removed()
    {
        _repository.Delete(3); // Active
        var task = _context.Tasks.Find(3)!; 
        task.State.Should().Be(State.Removed);
    }
    
    [Fact]
    public void Deleting_a_task_which_is_Resolved_Closed_or_Removed_should_return_Conflict()
    {
        var response = _repository.Delete(2); // Resolved
        response.Should().Be(Response.Conflict);

        response = _repository.Delete(4); // Closed 
        response.Should().Be(Response.Conflict);
        
        response = _repository.Delete(5); // Removed
        response.Should().Be(Response.Conflict);
    }
}