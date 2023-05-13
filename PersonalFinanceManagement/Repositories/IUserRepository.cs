using PersonalFinanceManagement.Models;

namespace PersonalFinanceManagement.Repositories;

public interface IUserRepository
{
    // User GetUserById(int id);
    void AddUser(User user);
    void UpdateUser(User user);
    void DeleteUser(Guid id);
}