using CarStore.AppContracts.Dtos;
using Microsoft.EntityFrameworkCore;
using ProductService.AppCore;
using ProductService.AppCore.Core;

namespace ProductService.Infrastructure.Data
{
    public class Repository : IRepository
    {
        private readonly MainDbContext _dbContext;

        public Repository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Add(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            _dbContext.Products.Add(product);

            await _dbContext.SaveChangesAsync();

            return product.Id;
        }

        public async Task<IEnumerable<ProductDto>> Get()
        {
            return await _dbContext.Products
                .Select(p => new ProductDto
                {
                    Name = p.Name,
                    Active = p.Active,
                    Cost = p.Cost,
                    Id = p.Id,
                    Quantity = p.Quantity,
                    Created = p.Created,
                    Updated = p.Updated,
                })
                .ToArrayAsync();
        }

        public async Task<ProductDto?> GetById(Guid id)
        {
            var entity = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return null;
            }

            return new ProductDto
            {
                Name = entity.Name,
                Active = entity.Active,
                Cost = entity.Cost,
                Id = entity.Id,
                Quantity = entity.Quantity,
                Created = entity.Created,
                Updated = entity.Updated,
            };
        }
    }
}
