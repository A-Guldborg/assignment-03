namespace Assignment3.Entities.Tests;

public class UserRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly SqliteConnection _connection;
    private readonly UserRepository _repository;
    
    public UserRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(_connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        // Repo specific adds
        var andreas = new User { Id = 1, Name = "Andreas Guldborg Hansen", Email = "aguh@itu.dk" };
        context.Users.Add(andreas);
        var andreasTask = new Task
        {
            Id = 1, Title = "Laundry", 
            AssignedTo = andreas, 
            Description = "Don't mix colors and white!",
            State = State.New
        };
        andreas.Tasks.Add(andreasTask);

        context.SaveChanges();
        
        _context = context;
        _repository = new UserRepository(context);
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }

    [Fact]
    public void Users_who_are_assigned_to_a_task_may_only_be_deleted_using_the_force()
    {
        var response = _repository.Delete(1, true);
        response.Should().Be(Response.Deleted);

        var entity = _context.Users.Find(1);
        entity.Should().BeNull();
    }

    [Fact]
    public void Trying_to_delete_a_user_in_use_without_the_force_should_return_Conflict()
    {
        var response = _repository.Delete(1, false);
        response.Should().Be(Response.Conflict);
        
        var entity = _context.Users.Find(1);
        entity.Should().NotBeNull();
    }
    
    [Fact]
    public void Trying_to_create_a_user_which_exists_already_same_email_should_return_Conflict()
    {
        // Assign
        var aguh_user_two = new UserCreateDTO("Andreas Test User", "aguh@itu.dk");

        // Act
        var (response, userId) = _repository.Create(aguh_user_two);
        
        // Assert
        response.Should().Be(Response.Conflict);
    }
}
