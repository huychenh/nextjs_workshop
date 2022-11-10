using CarStore.AppContracts.Dtos;
using Microsoft.EntityFrameworkCore;
using OrderingService.AppCore;
using OrderingService.AppCore.Core;

namespace OrderingService.Infrastructure.Data
{
    public class Repository : IOrderRepository
    {
        private readonly MainDbContext _dbContext;

        public Repository(MainDbContext dbContext)
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

        public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerId(Guid id)
        {
            var orders = await _dbContext.Orders
                .Where(x => x.BuyerId == id)
                .Select(x => new OrderDto
                {
                    Id = x.Id,
                    ProductId = x.ProductId,
                    OwnerId = x.OwnerId,
                    BuyerId = x.BuyerId,
                    Price = x.Price,
                    ProductName = x.ProductName,
                    PictureUrl = x.PictureUrl,
                })
                .ToListAsync();


            return orders;
        }
    }
}
