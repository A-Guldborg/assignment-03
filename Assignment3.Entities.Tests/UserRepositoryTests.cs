using Assignment3.Core;

namespace Assignment3.Entities.Tests;

public class UserRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        _repository = new UserRepository();
        _repository.Create(new UserCreateDTO("Andreas Guldborg Hansen", "aguh@itu.dk"));
        _repository.Create(new UserCreateDTO("William Skou Heidemann", "wihe@itu.dk"));
        _repository.Create(new UserCreateDTO("Rakul Maria Hjalmarsdóttir Tórgarð", "rakt@itu.dk"));
    }

    [Fact]
    public void Trying_to_create_a_user_which_exists_already_same_email_should_return_Conflict()
    {
        // Assign
        var user = new UserCreateDTO("Frederik Petersen", "aguh@itu.dk");
        
        // Act
        var returnTuple = _repository.Create(user);
        
        // Assert
        returnTuple.Response.Should().Be(Response.Conflict);
    }

    [Fact]
    public void Trying_to_delete_a_user_in_use_without_the_force_should_return_Conflict()
    {
        // Assign
        var william = _repository.Read(0); 
        new UserCreateDTO("Frederik Petersen", "frepe@itu.dk");
        

        // Act

        // Assert
    }
}
