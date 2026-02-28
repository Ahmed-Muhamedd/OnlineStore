using OnlineStore.Common;
using OnlineStore.Dtos;

namespace OnlineStore.Interfaces
{
    public interface IOrderService
    {
        Task<Result<OrderResponseDto>> CreateOrderAsync(CreateOrderDto CreateOrder , string customerId);
        Task<Result<OrderDetailsDto>> GetOrderByIdAsync(int orderId, string customerId);
        Task<Result<IEnumerable<OrderDetailsDto>>> GetCustomerOrdersAsync(string customerId);
        //CancelOrderAsync(string orderId, string customerId)
    }
}
