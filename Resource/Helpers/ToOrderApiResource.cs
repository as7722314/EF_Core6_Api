using CoreApiTest.Models;

namespace CoreApiTest.Resource.Helpers
{
    public class ToOrderApiResource
    {
        public OrderApiResource DoConvertForModel(Order order)
        {
            OrderApiResource orderApiResource = new()
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Total = order.Total,
                User = order.User is not null ? new UserApiResource
                {
                    Id = order.User.Id,
                    Name = order.User.Name,
                    Email = order.User.Email
                } : null
            };
            return orderApiResource;
        }

        public List<OrderApiResource> DoConvertForList(List<Order> orders)
        {
            List<OrderApiResource> orderApiResorces = new();
            foreach (var order in orders)
            {
                orderApiResorces.Add(DoConvertForModel(order));
            }
            return orderApiResorces;
        }
    }
}
