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
            StateUpdated = DateTime.UtcNow,
            Tags = _context.Tags.Where(t => task.Tags.Contains(t.Name)).ToList()
        };
        _context.Tasks.Add(newTask);
        _context.SaveChanges();
        return (Response.Created, newTask.Id);
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        var collection =
            from t in _context.Tasks
            select new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, 
                t.Tags.Select(v => v.Name).ToList().AsReadOnly(), t.State);

        return collection.ToList().AsReadOnly();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        var collection =
            from t in _context.Tasks
            where t.State == State.Removed
            select new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, 
                t.Tags.Select(v => v.ToString()).ToList().AsReadOnly(), t.State);

        return collection.ToList();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        var collection =
            from t in _context.Tasks
            where t.Tags.Select(taskTag => taskTag.ToString().Equals(tag)).Any()
            select new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, 
                t.Tags.Select(v => v.ToString()).ToList().AsReadOnly(), t.State);
        
        return collection.ToList().AsReadOnly();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        var collection =
            from t in _context.Tasks
            where t.AssignedTo.Id == userId
            select new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, 
                t.Tags.Select(v => v.ToString()).ToList().AsReadOnly(), t.State);

        return collection.ToList().AsReadOnly();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(Core.State state)
    {
        var collection =
            from t in _context.Tasks
            where t.State == state
            select new TaskDTO(t.Id, t.Title, t.AssignedTo.Name, 
                t.Tags.Select(v => v.ToString()).ToList().AsReadOnly(), t.State);

        return collection.ToList().AsReadOnly();
    }

    public TaskDetailsDTO Read(int taskId)
    {
        //var task = _context.Tasks.Find(taskId);
        var task = _context.Tasks.FirstOrDefault(t => t.Id == taskId);
        return new TaskDetailsDTO(task.Id, task.Title, task.Description, task.Created, 
            task.AssignedTo is null ? null : task.AssignedTo.Name, 
            task.Tags.Select(v => v.ToString()).ToList().AsReadOnly(),
            task.State, task.StateUpdated);
    }

    public Response Update(TaskUpdateDTO task)
    {
        var entity = _context.Tasks.FirstOrDefault(t => t.Id == task.Id);
        if (entity is null) return Response.NotFound;
        entity.Id = task.Id;
        entity.Title = task.Title;
        var user = _context.Users.Find(task.AssignedToId);
        if (user is null) return Response.BadRequest;
        entity.AssignedTo = user;
        entity.Description = task.Description;
        entity.State = task.State;
        entity.StateUpdated = DateTime.UtcNow;
        entity.Tags = _context.Tags.Where(t => task.Tags.Contains(t.Name)).ToList();
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
