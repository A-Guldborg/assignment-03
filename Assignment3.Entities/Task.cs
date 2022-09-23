using System.ComponentModel.DataAnnotations;

namespace Assignment3.Entities;

public class Task
{
    public int Id { get; set; }
    [Required] public string Title { get; set; } = "Anonymous Task";
    public User? AssignedTo { get; set; }
    public string? Description { get; set; }
    [Required] public State State { get; set; }
    public Tag[] Tags { get; set; } = new Tag[0];
}

public enum State
{
    New,
    Active,
    Resolved,
    Closed,
    Removed
}