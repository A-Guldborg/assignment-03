namespace Assignment3.Entities.Tests;

public class TagRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly TagRepository _repository;
    public TagRepositoryTests() {
        var builder = new DbContextOptionsBuilder<KanbanContext>()
            .UseInMemoryDatabase("KanbanTest")
            .ConfigureWarnings(b => 
                b.Ignore(InMemoryEventId.TransactionIgnoredWarning)).Options;
        using var context = new KanbanContext(builder);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        // Repo specific adds
        var laundryTask = new Task{
            Id = 1,
            Title = "Laundry",
            State = State.New,
        };

        var houseworkTag = new Tag{
            Id = 1,
            Name = "Housework"
        };
        
        laundryTask.Tags.Append(houseworkTag);
        houseworkTag.Tasks.Append(laundryTask);


        context.SaveChanges();
        
        _context = context;
        _repository = new();
    }

    [Fact]
    public void Trying_to_delete_a_tag_in_use_without_the_force_should_return_Conflict()
    {
        // Given
        var response = _repository.Delete(1, false);
        // When
    
        // Then
        response.Should().Be(Response.Conflict);
    }

    [Fact]
    public void Tags_which_are_assigned_to_a_task_may_only_be_deleted_using_the_force()
    {
        // Given
        var response = _repository.Delete(1, true);
    
        // When
    
        // Then
        response.Should().Be(Response.Deleted);
    }

    [Fact]
    public void Trying_to_create_a_tag_which_exists_already_should_return_Conflict()
    {
        // Given
        var duplicateTag = new Tag{
            Id = 2,
            Name = "Housework"
        };
    
        // When
        var response = _context.Tags.Add(duplicateTag);
    
        // Then
        response.Should().Be(Response.Conflict);
    }
}
