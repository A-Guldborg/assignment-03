using System.Collections;
using System.Configuration;
using Assignment3.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;

namespace Assignment3.Entities.Tests;

public class TaskRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly TaskRepository _repository;
    private static readonly InMemoryDatabaseRoot _databaseRoot;

    public TaskRepositoryTests()
    {
        var builder = new DbContextOptionsBuilder<KanbanContext>()
            .UseInMemoryDatabase("KanbanTest", _databaseRoot)
            .ConfigureWarnings(b => 
                b.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
        var context = new KanbanContext(builder);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        // Repo specific adds
        var newTask = new Task
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
        
        var andreas = new User { Id = 1, Name = "Andreas Guldborg Hansen", Email = "aguh@itu.dk" };
        context.Users.Add(andreas);
        var andreasTask = new Task
        {
            Id = 6, Title = "Laundry", 
            AssignedTo = andreas, 
            Description = "Don't mix colors and white!",
            State = State.New
        };
        andreas.Tasks.Add(andreasTask);

        context.AddRange(newTask, resolvedTask, activeTask, closedTask, removedTask);

        context.SaveChanges();
        
        _context = context;
        _repository = new TaskRepository(context);
    }
    
    public void Dispose()
    {
        _context.Dispose();
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

    [Fact]
    public void Creating_a_task_will_set_its_state_to_New_and_Created_and_StateUpdated_to_current_time_in_UTC()
    {
        var response = _repository.Create(new TaskCreateDTO("Monday Task", null, "", new List<string>()));
        var task = _repository.Read(response.TaskId);
        task.State.Should().Be(Core.State.New);
        
        var expected = DateTime.UtcNow;
        
        task.Created.Should().BeCloseTo(expected, precision: TimeSpan.FromSeconds(5));
        task.StateUpdated.Should().BeCloseTo(expected, precision: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Create_or_update_task_must_allow_for_editing_tags()
    {
        var tag_repository = new TagRepository(_context);
        tag_repository.Create(new TagCreateDTO("Important"));
        tag_repository.Create(new TagCreateDTO("A"));

        var response = _repository.Create(new TaskCreateDTO("Test Task", 1, "", new List<string> {"Important", "A"} ));
        var task = _repository.Read(response.TaskId);
        task.Tags.Count.Should().Be(2);
        
        
        tag_repository.Create(new TagCreateDTO("Boring"));
        var updateDTO = new TaskUpdateDTO(task.Id, task.Title, 1, task.Description, new List<string>() {"Important", "A", "Boring"}, task.State);
        _repository.Update(updateDTO);
        task = _repository.Read(task.Id);
        task.Tags.Count.Should().Be(3);
    }

    [Fact]
    public void Updating_the_State_of_a_task_will_change_the_StateUpdated_to_current_time_in_UTC()
    {
        var task = _context.Tasks.Find(3); // Active task
        _repository.Update(new TaskUpdateDTO(task.Id, task.Title, 1, task.Description, new string[] { }, task.State));
        var taskDetails = _repository.Read(task.Id);
        var expected = DateTime.UtcNow;
        taskDetails.StateUpdated.Should().BeCloseTo(expected, precision: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Assigning_a_user_which_does_not_exist_should_return_BadRequest()
    {
        var task = _context.Tasks.Find(3); // Active task
        var response = _repository.Update(new TaskUpdateDTO(task.Id, task.Title, 2, task.Description, new string[] { }, task.State)); // User id 2 does not exist
        response.Should().Be(Response.BadRequest);
    }
}