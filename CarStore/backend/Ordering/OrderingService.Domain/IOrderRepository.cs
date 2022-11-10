
using CarStore.AppContracts.Dtos;
using OrderingService.AppCore.Core;

namespace OrderingService.AppCore
{
    public interface IOrderRepository
    {
        Task<Guid> Add(Order brand);
        Task<IEnumerable<OrderDto>> GetOrdersByCustomerId(Guid id);
    }
}