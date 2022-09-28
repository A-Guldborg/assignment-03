using System.Collections.ObjectModel;
using Assignment3.Core;

namespace Assignment3.Entities;

public class UserRepository : IUserRepository
{
    private readonly KanbanContext _context;
    
    public UserRepository(KanbanContext context)
    {
        _context = context;
    }

    public (Response Response, int UserId) Create(UserCreateDTO user)
    {
        var entity = _context.Users.FirstOrDefault(u => u.Email == user.Email);
        if (entity is not null) return (Response.Conflict, entity.Id);
        var newUser = new User();
        newUser.Id = _context.Users.Count();
        newUser.Email = user.Email;
        newUser.Name = user.Name;
        _context.Users.Add(newUser);
        _context.SaveChanges();
        return (Response.Created, newUser.Id);
    }

    public IReadOnlyCollection<UserDTO> ReadAll()
    {
        var userDTOs = 
            from user in _context.Users
            orderby user.Id
            select new UserDTO(user.Id, user.Name, user.Email);
        
        return userDTOs.ToArray();
    }

    public UserDTO? Read(int userId)
    {
        var userDTOs = 
            from user in _context.Users
            where user.Id == userId
            select new UserDTO(user.Id, user.Name, user.Email);
        
        return userDTOs.FirstOrDefault();
    }

    public Response Update(UserUpdateDTO user)
    {
        var entity = _context.Users.Find(user.Id);
        if (entity is null) return Response.NotFound;
        if (user.Email == entity.Email && user.Name == entity.Name) return Response.Conflict;
        entity.Email = user.Email;
        entity.Name = user.Name;
        _context.SaveChanges();
        return Response.Updated;
    }

    public Response Delete(int userId, bool force = false)
    {
        var user = _context.Users.Find(userId);
        if (user is null) return Response.NotFound;
        if (user.Tasks.Count > 0 && force == false) return Response.Conflict;
        _context.Users.Remove(user);
        _context.SaveChanges();
        return Response.Deleted;
    }
}
