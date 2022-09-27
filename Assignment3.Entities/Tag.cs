namespace Assignment3.Entities;

public class Tag
{
    public Tag(string name) {
        Name = name;
    }
    public int Id { get; set; }
    public string Name { get; set; } = "sys_auto";
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
    
}
