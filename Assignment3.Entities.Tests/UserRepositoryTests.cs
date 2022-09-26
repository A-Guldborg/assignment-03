using Assignment3.Core;

namespace Assignment3.Entities.Tests;

public class UserRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        _repository = new UserRepository();
        //_repository.Create(new UserCreateDTO("Andreas Guldborg Hansen", "aguh@itu.dk"));
        //_repository.Create(new UserCreateDTO("William Skou Heidemann", "wihe@itu.dk"));
        //_repository.Create(new UserCreateDTO("Rakul Maria Hjalmarsdóttir Tórgarð", "rakt@itu.dk"));
    }

    [Fact]
    public void Users_who_are_assigned_to_a_task_may_only_be_deleted_using_the_force()
    {
        // // Assign
        // // var user = new User()
        // // {
        // //     Name = "Test_Name",
        // //     Email = "test@test.com",
        // // };
        //
        // var user = _context.Users.Find(1);
        //
        // var task = new Task()
        // {
        //     Title = "Test_Title",
        //     State = State.Active,
        //     AssignedTo = user,
        // };
        //
        //
        // user.Tasks.Add(task);
        //
        // // Act
        // var response = _repository.Delete(user.Id);
        //
        //
        // // Assert


    }

    [Fact]
    public void Trying_to_delete_a_user_in_use_without_the_force_should_return_Conflict()
    {
        // // Assign
        // var user = new UserCreateDTO("Test_Name", "test@test.com");
        //
        // var task = new Task()
        // {
        //     Title = "Work on a sunday",
        //     State = State.Active,
        //     AssignedTo = user
        // };
        //
        // user.Tasks.Add(task);
        //
        // // Act
        // _repository.Create(user);
        // var response = _repository.Delete(user.Id, false);
        //
        // // Assert
        // response.Should().Be(Response.Conflict);
    }
    
    [Fact]
    public void Trying_to_create_a_user_which_exists_already_same_email_should_return_Conflict()
    {
        // Assign
        var aguh_user_one = new UserCreateDTO("Andreas Guldborg Hansen", "aguh@itu.dk");
        var aguh_user_two = new UserCreateDTO("Frederik Petersen", "aguh@itu.dk");

        // Act
        _repository.Create(aguh_user_one);
        var returnTuple = _repository.Create(aguh_user_two);
        
        // Assert
        returnTuple.Response.Should().Be(Response.Conflict);
    }
}
