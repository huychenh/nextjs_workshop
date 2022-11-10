using CarStore.AppContracts.Dtos;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OrderingService.AppCore;
using OrderingService.AppCore.Core;

namespace OrderingService.Infrastructure.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly MainDbContext _dbContext;

        public OrderRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Add(Order order)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order));
            }

            _dbContext.Orders.Add(order);

            await _dbContext.SaveChangesAsync();

            return order.Id;
        }

        public Task<IEnumerable<OrderDto>> GetOrdersByCustomerId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
