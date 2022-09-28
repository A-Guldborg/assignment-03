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
        throw new NotImplementedException();
    }

    public Response Delete(int taskId)
    {
        throw new NotImplementedException();
    }
}
