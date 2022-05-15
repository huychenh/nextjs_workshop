using CarStore.AppContracts.Dtos;
using ProductService.AppCore.Core;

namespace ProductService.AppCore
{
    public interface IRepository
    {
        Task<IEnumerable<ProductDto>> Get(string text);

        Task<ProductDto?> GetById(Guid id);

        Task<Guid> Add(Product product);
    }
}
