using System.ComponentModel.DataAnnotations;

namespace Assignment3.Entities;

public class User
{
    public int Id { get; set; }
    [Required] public string Name { get; set; } = "John Doe";
    [Required] public string Email { get; set; } = "JohnDoe@gmail.com";
    public Task[] Tasks { get; set; } = new Task[0];
}
