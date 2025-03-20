using myProj.Models;

namespace myProj.interfaces;

public interface IUserService
{
    List<User> GetUsers();
    User GetUser(int id);
    int Insert(User newBook);
    bool Update(int id, User newBook);
    bool Delete(int id);
}
