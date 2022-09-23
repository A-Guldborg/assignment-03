namespace Assignment3.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "John Doe";
    public string Email { get; set; } = "JohnDoe@gmail.com";
    public Task[] Tasks { get; set; } = new Task[0];
}
