using ProductService.AppCore.Core;
using ProductService.AppCore.UseCases.Queries;
using ProductService.Shared.DTO;

namespace ProductService.AppCore
{
    public interface IBrandRepository
    {
        Task<IEnumerable<BrandDto>> Get(GetBrands queryDto);

        Task<BrandDto?> GetByName(string name);

        Task<Guid> Add(Brand brand);
    }
}
