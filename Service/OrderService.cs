using CoreApiTest.Data;
using CoreApiTest.Interface;
using CoreApiTest.Models;
using Microsoft.EntityFrameworkCore;
using CoreApiTest.Resource.Helpers;
using Microsoft.Data.SqlClient;
using static NuGet.Packaging.PackagingConstants;
using System.Security.Cryptography.X509Certificates;
using System.Data.Common;

namespace CoreApiTest.Service
{
    public class OrderService : IOrderService
    {
        private readonly CoreApiTestContext _coreApiTestContext;

        public OrderService(CoreApiTestContext coreApiTestContext)
        {
            _coreApiTestContext = coreApiTestContext;
        }

        public async Task<Order> CreateOrder(int userId, Order order)
        {
            var transation = _coreApiTestContext.Database.BeginTransaction();
            try
            {
                User user = await _coreApiTestContext.Users
                    .Include(b => b.Orders)
                    .Where(u => u.Id == userId)
                    .FirstAsync();

                user.Orders?.Add(order);

                await _coreApiTestContext.SaveChangesAsync();

                transation.Commit();

                return order;
            }
            catch (Exception)
            {
                transation.Rollback();
                throw;
            }
        }

        public async Task<Order> GetOrderById(int id)
        {
            var order = await _coreApiTestContext.Orders.Include(o => o.User).FirstAsync(o => o.Id == id);
            return order;
        }

        public async Task<List<Order>> GetOrders(string sortName, string sort)
        {
            List<Order> orders = new();

            switch (sortName + "_" + sort)
            {
                case "id_desc":
                    orders = await _coreApiTestContext.Orders.Include(o => o.User).OrderByDescending(o => o.Id).ToListAsync();
                    break;

                case "id_asc":
                    orders = await _coreApiTestContext.Orders.Include(o => o.User).OrderBy(o => o.Id).ToListAsync();
                    break;
            }
            return orders;
        }
    }
}