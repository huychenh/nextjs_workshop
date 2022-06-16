﻿using CarStore.AppContracts.Dtos;
using CarStore.IntegrationEvents.Brand;
using N8T.Core.Domain;

namespace ProductService.AppCore.Core
{
    public class Brand : EntityRootBase
    {
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }

        public static Brand Create(BrandCreateDto dto)
        {
            var brand = new Brand
            {
                Name = dto.Name
            };

            brand.AddDomainEvent(new BrandCreatedIntegrationEvent
            {
                Id = brand.Id
            });

            return brand;
        }
    }
}