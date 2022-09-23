namespace Assignment3.Entities;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; }
    public User AssignedTo { get; set; }
    public string Description { get; set; }
    public State State { get; set; }
    public Tag[] Tags { get; set; }
}

public enum State
{
    New,
    Active,
    Resolved,
    Closed,
    Removed
}