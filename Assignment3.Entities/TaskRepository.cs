using System.Collections.ObjectModel;
using Assignment3.Core;

namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{
    private readonly KanbanContext _context;
    
    public TaskRepository(KanbanContext context)
    {
        _context = context;
    }
    
    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        var newTask = new Task
        { 
            Title = task.Title, 
            AssignedTo = _context.Users.Find(task.AssignedToId), 
            Description = task.Description, 
            State = State.New,
            StateUpdated = DateTime.UtcNow
        };
        _context.Tasks.Add(newTask);
        _context.SaveChanges();
        return (Response.Created, newTask.Id);
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        // er der ikke en bedre måde at gøre dether på ?
        //
        // var tags = new Collection<string>();
        // foreach (var tag in _context.Tags)
        // {
        //     tags.Add(tag.ToString());
        // }
        // IReadOnlyCollection<string> taskTags = tags;

        var collection =
            from t in _context.Tasks
            select new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, ???, t.State);

        return collection.ToList();
                        
    }

    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        var tags = new Collection<string>();
        foreach (var tag in _context.Tags)
        {
            tags.Add(tag.ToString());
        }
        IReadOnlyCollection<string> taskTags = tags;

        var collection =
            from t in _context.Tasks
            where t.State == State.Removed
            select new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, taskTags, t.State);

        return collection.ToList();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        var collection =
            from t in _context.Tasks
            where t.Tags.Equals(tag)
            select new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, ???, t.State);
        
        return collection.ToList();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        var collection =
            from t in _context.Tasks
            where t.AssignedTo.Id == userId
            select new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, ???, t.State);

        return collection.ToArray();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(Core.State state)
    {
        var collection =
            from t in _context.Tasks
            where t.State == state
            select new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, ???, t.State);

        return collection.ToArray();
    }

    public TaskDetailsDTO Read(int taskId)
    {
    
        var task = _context.Tasks.Find(taskId);

        var tags = new Collection<string>();
        foreach (var tag in task.Tags)
        {
            tags.Add(tag.ToString());
        }
        IReadOnlyCollection<string> taskTags = tags;

        return new TaskDetailsDTO(task.Id, task.Title, task.Description, task.Created, task.AssignedTo.Name, taskTags,
        task.State, task.StateUpdated);
    }

    public Response Update(TaskUpdateDTO task)
    {
        var entity = _context.Tasks.FirstOrDefault(t => t.Id == task.Id);
        if (entity is null) return Response.NotFound;
        entity.Id = task.Id;
        entity.Title = task.Title;
        entity.AssignedTo = _context.Users.Find(task.AssignedToId);
        entity.Description = task.Description;
        entity.State = task.State;
        entity.StateUpdated = DateTime.UtcNow;
        _context.SaveChanges();
        return Response.Updated;
    }

    public Response Delete(int taskId)
    {
        var entity = _context.Tasks.Find(taskId);
        if (entity is null) return Response.NotFound;
        if (entity.State == State.Resolved || entity.State == State.Closed || entity.State == State.Removed) return Response.Conflict;
        if (entity.State == State.Active) entity.State = State.Removed;
        return Response.Deleted;
    }
}
