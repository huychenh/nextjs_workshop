using Microsoft.EntityFrameworkCore;
using N8T.Core.Domain;
using ProductService.AppCore;
using ProductService.AppCore.Core;
using ProductService.AppCore.UseCases.Queries;
using ProductService.Shared.DTO;

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

        public async Task<IEnumerable<ProductDto>> Get(GetProducts request)
        {
            IQueryable<Product> query = _dbContext.Products;
            var forSpecificBrand = !string.IsNullOrEmpty(request.Brand);

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                var lowerText = request.SearchText.ToLower();
                if (!forSpecificBrand)
                {
                    query = query.Where(x => x.Name.ToLower().Contains(lowerText)
                      || x.Brand.Name.ToLower().Contains(lowerText)
                      || x.Model.ToLower().Contains(lowerText)
                      || x.Year.ToString().Contains(lowerText));
                }
                else
                {
                    query = query.Where(x => x.Name.ToLower().Contains(lowerText)
                                          || x.Model.ToLower().Contains(lowerText)
                                          || x.Year.ToString().Contains(lowerText));
                }
            }
            if (forSpecificBrand)
            {
                query = query.Where(x => x.Brand.Name.ToLower().Equals(request.Brand.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.CategoryName))
            {
                query = query.Where(x => request.CategoryName.Contains(x.Category));
            }
            if (request.PriceFrom > 0 && request.PriceTo > 0)
            {
                query = query.Where(x => x.Price >= request.PriceFrom && x.Price <= request.PriceTo);
            }
            if (request.LatestNews.HasValue && request.LatestNews == true)
            {
                query = query.OrderByDescending(x => x.Year);
            }
            if (request.LowestPrice.HasValue && request.LowestPrice == true)
            {
                query = query.OrderBy(x => x.Price);
            }

            //Todo: get OwnerName
            return await query.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Brand = p.Brand.Name,
                Category = p.Category,
                Color = p.Color,
                Description = p.Description,
                FuelType = p.FuelType.ToString(),
                HasInstallment = p.HasInstallment,
                KmDriven = p.KmDriven,
                MadeIn = p.MadeIn,
                Model = p.Model,
                OwnerId = p.OwnerId.ToString(),
                Price = p.Price,
                SeatingCapacity = p.SeatingCapacity,
                Transmission = p.Transmission.ToString(),
                Verified = p.Verified,
                Year = p.Year,
                Active = p.Active,
                Created = p.Created,
                Updated = p.Updated,
            })
            .ToArrayAsync();
        }

        public async Task<ProductDto?> GetById(Guid id)
        {
            var entity = await _dbContext.Products.Include(x => x.Brand)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
            {
                return null;
            }

            return new ProductDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Brand = entity.Brand.Name,
                Category = entity.Category,
                Color = entity.Color,
                Description = entity.Description,
                FuelType = entity.FuelType.ToString(),
                HasInstallment = entity.HasInstallment,
                KmDriven = entity.KmDriven,
                MadeIn = entity.MadeIn,
                Model = entity.Model,
                OwnerId = entity.OwnerId.ToString(), //Todo: get OwnerName
                Price = entity.Price,
                SeatingCapacity = entity.SeatingCapacity,
                Transmission = entity.Transmission.ToString(),
                Verified = entity.Verified,
                Year = entity.Year,
                Active = entity.Active,
                Created = entity.Created,
                Updated = entity.Updated,
                Images = entity.Images?.ToArray(),
            };
        }

        public async Task<ListResultModel<ProductDto>> GetWithPagination(GetProducts request)
        {
            IQueryable<Product> query = _dbContext.Products;
            var forSpecificBrand = !string.IsNullOrEmpty(request.Brand);

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                var lowerText = request.SearchText.ToLower();
                if (!forSpecificBrand)
                {
                    query = query.Where(x => x.Name.ToLower().Contains(lowerText)
                      || x.Brand.Name.ToLower().Contains(lowerText)
                      || x.Model.ToLower().Contains(lowerText)
                      || x.Year.ToString().Contains(lowerText));
                }
                else
                {
                    query = query.Where(x => x.Name.ToLower().Contains(lowerText)
                                          || x.Model.ToLower().Contains(lowerText)
                                          || x.Year.ToString().Contains(lowerText));
                }
            }

            if (forSpecificBrand)
            {
                query = query.Where(x => x.Brand.Name.ToLower().Equals(request.Brand.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.CategoryName))
            {
                query = query.Where(x => request.CategoryName.Contains(x.Category));
            }

            if (request.PriceFrom > 0 && request.PriceTo > 0)
            {
                query = query.Where(x => x.Price >= request.PriceFrom && x.Price <= request.PriceTo);
            }

            if (request.LatestNews.HasValue && request.LatestNews == true)
            {
                query = query.OrderByDescending(x => x.Year);
            }

            if (request.LowestPrice.HasValue && request.LowestPrice == true)
            {
                query = query.OrderBy(x => x.Price);
            }

            var totalRecords = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalRecords / request.PageSize);
            var items = await query
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Brand = p.Brand.Name,
                    Category = p.Category,
                    Color = p.Color,
                    Description = p.Description,
                    FuelType = p.FuelType.ToString(),
                    HasInstallment = p.HasInstallment,
                    KmDriven = p.KmDriven,
                    MadeIn = p.MadeIn,
                    Model = p.Model,
                    OwnerId = p.OwnerId.ToString(),
                    Price = p.Price,
                    SeatingCapacity = p.SeatingCapacity,
                    Transmission = p.Transmission.ToString(),
                    Verified = p.Verified,
                    Year = p.Year,
                    Active = p.Active,
                    Created = p.Created,
                    Updated = p.Updated,
                    Images = p.Images != null ? p.Images.ToArray() : Array.Empty<string>(),
                })
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArrayAsync();

            return ListResultModel<ProductDto>.Create(items, totalPages, request.Page, request.PageSize);
        }
    }
}
