﻿using CarStore.AppContracts.Dtos;
using Microsoft.EntityFrameworkCore;
using ProductService.AppCore;
using ProductService.AppCore.Core;
using System.Linq;

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

        public async Task<IEnumerable<ProductDto>> Get(SearchProductDto queryDto)
        {
            IQueryable<Product> query = _dbContext.Products;
            var forSpecificBrand = !string.IsNullOrEmpty(queryDto.Brand);

            if (!string.IsNullOrEmpty(queryDto.SearchText))
            {
                var lowerText = queryDto.SearchText.ToLower();
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
                query = query.Where(x => x.Brand.Name.ToLower().Equals(queryDto.Brand.ToLower()));
            }
            if (!string.IsNullOrEmpty(queryDto.CategoryName))
            {
                query = query.Where(x => x.Category.ToLower() == queryDto.CategoryName.ToLower());
            }
            if (queryDto.PriceFrom > 0 && queryDto.PriceTo > 0)
            {
                query = query.Where(x => x.Price >= queryDto.PriceFrom && x.Price <= queryDto.PriceTo);
            }
            if (queryDto.Created.HasValue)
            {
                query = query.Where(x => x.Created <= TimeZoneInfo.ConvertTimeToUtc(queryDto.Created.Value));
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
                OwnerName = p.OwnerId.ToString(),
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
                OwnerName = entity.OwnerId.ToString(), //Todo: get OwnerName
                Price = entity.Price,
                SeatingCapacity = entity.SeatingCapacity,
                Transmission = entity.Transmission.ToString(),
                Verified = entity.Verified,
                Year = entity.Year,
                Active = entity.Active,
                Created = entity.Created,
                Updated = entity.Updated,
            };
        }

        public async Task<IEnumerable<ProductDto>> GetWithPagination(SearchProductDto queryDto)
        {
            IQueryable<Product> query = _dbContext.Products;

            if (!string.IsNullOrEmpty(queryDto.SearchText))
            {
                var lowerText = queryDto.SearchText.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(lowerText) ||
                    x.Brand.ToLower().Contains(lowerText) ||
                    x.Model.ToLower().Contains(lowerText) ||
                    x.Year.ToString().Contains(lowerText));
            }
            if (queryDto.PriceFrom > 0 && queryDto.PriceTo > 0)
            {
                query = query.Where(x => x.Price >= queryDto.PriceFrom && x.Price <= queryDto.PriceTo);
            }
            if (queryDto.Created.HasValue)
            {
                query = query.Where(x => x.Created <= TimeZoneInfo.ConvertTimeToUtc(queryDto.Created.Value));
            }

            var allCars = query.ToList();
            int extend = (int)(allCars.Count % queryDto.PageSize);

            return await query.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Brand = p.Brand,
                Category = p.Category,
                Color = p.Color,
                Description = p.Description,
                FuelType = p.FuelType.ToString(),
                HasInstallment = p.HasInstallment,
                KmDriven = p.KmDriven,
                MadeIn = p.MadeIn,
                Model = p.Model,
                OwnerName = p.OwnerId.ToString(),
                Price = p.Price,
                SeatingCapacity = p.SeatingCapacity,
                Transmission = p.Transmission.ToString(),
                Verified = p.Verified,
                Year = p.Year,
                Active = p.Active,
                Created = p.Created,
                Updated = p.Updated,
                TotalPages = (int)(allCars.Count / queryDto.PageSize + (extend == 0 ? 0 : 1))
            })
                .Skip((int)((queryDto.Page - 1) * queryDto.PageSize))
                .Take((int)queryDto.PageSize)
                .ToArrayAsync();
        }
    }
}
