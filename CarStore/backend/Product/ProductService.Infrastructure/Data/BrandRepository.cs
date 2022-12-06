using Microsoft.EntityFrameworkCore;
using ProductService.AppCore;
using ProductService.AppCore.Core;
using ProductService.AppCore.UseCases.Queries;
using ProductService.Shared.DTO;

namespace ProductService.Infrastructure.Data
{
    public class BrandRepository : IBrandRepository
    {
        private readonly MainDbContext _dbContext;

        public BrandRepository(MainDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Add(Brand brand)
        {
            if (brand == null)
            {
                throw new ArgumentNullException(nameof(brand));
            }

            _dbContext.Brands.Add(brand);

            await _dbContext.SaveChangesAsync();

            return brand.Id;
        }

        public async Task<IEnumerable<BrandDto>> Get(GetBrands queryDto)
        {
            IQueryable<Brand> query = _dbContext.Brands;

            if (!string.IsNullOrEmpty(queryDto.SearchText))
            {
                var searchText = queryDto.SearchText.Trim();
                query = query.Where(x => x.Name.Contains(searchText));
            }

            return await query
                .Select(p => new BrandDto
                {
                    Id = p.Id,
                    Name = p.Name,
                })
                .ToArrayAsync();
        }

        public async Task<BrandDto?> GetByName(string name)
        {
            name = name.ToLower().Trim();
            var entity = await _dbContext.Brands.FirstOrDefaultAsync(x => x.Name.ToLower().Equals(name));

            if (entity == null)
            {
                return null;
            }

            return new BrandDto
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}
