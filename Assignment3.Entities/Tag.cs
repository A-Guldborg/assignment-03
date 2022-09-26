namespace Assignment3.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = "sys_auto";
    public IEnumerable<Task> Tasks { get; set; } = new List<Task>();
    
}
