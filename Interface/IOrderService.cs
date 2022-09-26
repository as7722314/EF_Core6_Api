using CoreApiTest.Models;

namespace CoreApiTest.Interface
{
    public interface IOrderService
    {
        Task<Order> CreateOrder(int userId, Order order);
        Task<List<Order>> GetOrders(string sortName, string sort);
        Task<Order> GetOrderById(int id);

    }
}
