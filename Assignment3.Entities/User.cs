namespace Assignment3.Entities;

public class User
{
    private int Id { get; set; }
    private string Name { get; set; }
    private string Email { get; set; }
    private Task[] Tasks { get; set; }
}
