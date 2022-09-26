using CoreApiTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiTest.Interface
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsers();

        Task<User?> GetUserById(int id);

        Task<User> CreateUser(User users);

        Task<User> UpdateUser(User o_user, User users);

        void DeleteUser(User users);

        Task<User?> GetUserOnLogin(string account);
    }
}
