namespace Assignment3.Entities;

public enum State
{
    New,
    Active,
    Resolved,
    Closed,
    Removed
}

public class Task
{
    private int Id { get; set; }
    private string Title { get; set; }
    private User AssignedTo { get; set; }
    private string Description { get; set; }
    private State State { get; set; }
    private Tag[] Tags { get; set; }
}
