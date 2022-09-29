using System.ComponentModel.DataAnnotations;
using Assignment3.Core;

namespace Assignment3.Entities;

public class Task
{
    public int Id { get; set; }
    [Required] public string Title { get; set; } = "Anonymous Task";
    public User? AssignedTo { get; set; }
    public string? Description { get; set; }
    [Required] public State State { get; set; }
    public DateTime StateUpdated { get; set; } = DateTime.UtcNow;
    public IEnumerable<Tag> Tags { get; set; } = new List<Tag>();
    public DateTime Created { get; } = DateTime.UtcNow;
    
}

