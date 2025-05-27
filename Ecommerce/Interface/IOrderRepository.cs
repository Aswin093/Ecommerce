using Ecommerce.Model;
using System.Collections.Generic;

namespace Ecommerce.Interface
{
    public interface IOrderRepository
    {
        int CreateOrder(OrderRequestDto orderRequest);
        List<Order> GetOrdersByCustomer(int customerId);
        int DeleteOrder(int orderId);
    }
}