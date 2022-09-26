using System.Collections.ObjectModel;
using Assignment3.Core;

namespace Assignment3.Entities;

public class UserRepository : IUserRepository
{
    private Collection<UserDTO> _userDtos = new();

    public (Response Response, int UserId) Create(UserCreateDTO user)
    {
        var newUser = new UserDTO(_userDtos.Count, user.Name, user.Email);

        var temp = from u in _userDtos
            where u.Email == newUser.Email
            select new
            {
                u.Email
            };
        
        if (temp.Any()) 
            return (Response.Conflict, newUser.Id);

        _userDtos.Add(newUser);
        return (Response.Created, newUser.Id);
    }

    public IReadOnlyCollection<UserDTO> ReadAll()
    {
        throw new NotImplementedException();
    }

    public UserDTO Read(int userId)
    {
        throw new NotImplementedException();
    }

    public Response Update(UserUpdateDTO user)
    {
        throw new NotImplementedException();
    }

    public Response Delete(int userId, bool force = false)
    {
        throw new NotImplementedException();
    }
}
