using CarStore.AppContracts.Dtos;
using ProductService.AppCore.Core;

namespace ProductService.AppCore
{
    public interface IBrandRepository
    {
        Task<IEnumerable<BrandDto>> Get(SearchBrandDto queryDto);

        Task<BrandDto?> GetByName(string name);

        Task<Guid> Add(Brand brand);
    }
}
