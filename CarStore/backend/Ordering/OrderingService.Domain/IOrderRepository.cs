
using OrderingService.AppCore.Core;

namespace OrderingService.AppCore
{
    public interface IOrderRepository
    {
        Task<Guid> Add(Order brand);
    }
}