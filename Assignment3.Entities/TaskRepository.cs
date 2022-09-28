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
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(Core.State state)
    {
        throw new NotImplementedException();
    }

    public TaskDetailsDTO Read(int taskId)
    {
        throw new NotImplementedException();
    }

    public Response Update(TaskUpdateDTO task)
    {
        throw new NotImplementedException();
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
