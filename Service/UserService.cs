using CoreApiTest.Interface;
using CoreApiTest.Models;
using CoreApiTest.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CoreApiTest.Service
{
    public class UserService : IUserService
    {
        private readonly CoreApiTestContext _coreApiTestContext;
        public UserService(CoreApiTestContext coreApiTestContext)
        {
            _coreApiTestContext = coreApiTestContext;
        }

        public async Task<User> CreateUser(User user)
        {
            var transation = _coreApiTestContext.Database.BeginTransaction();
            try
            {
                _coreApiTestContext.Add(user);
                await _coreApiTestContext.SaveChangesAsync();

                transation.Commit();

                return user;
            }
            catch (Exception)
            {
                transation.Rollback();
                throw ;
            }
        }

        public void DeleteUser(User user)
        {
            try
            {
                _coreApiTestContext.Remove(user);
                _coreApiTestContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw ;
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            var users = await _coreApiTestContext.Users.OrderByDescending(u => u.Id).ToArrayAsync();
            return users.ToList() ;
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _coreApiTestContext.Users.SingleOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserOnLogin(string account)
        {
            return await _coreApiTestContext.Users.Where(u => u.Account == account).FirstOrDefaultAsync();
        }

        public async Task<User> UpdateUser(User o_user, User user)
        {
            var transation = _coreApiTestContext.Database.BeginTransaction();
            try
            {
                o_user.Name = user.Name;
                await _coreApiTestContext.SaveChangesAsync();
                transation.Commit();
                return o_user;
            }
            catch (Exception)
            {
                transation.Rollback();
                throw;
            }
        }
    }
}
