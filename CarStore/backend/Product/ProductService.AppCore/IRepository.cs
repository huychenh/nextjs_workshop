using N8T.Core.Domain;
using ProductService.AppCore.Core;
using ProductService.Shared.DTO;

namespace ProductService.AppCore
{
    public interface IRepository
    {
        Task<ListResultModel<ProductDto>> GetWithPagination(SearchProductDto queryDto);

        Task<ProductDto?> GetById(Guid id);

        Task<Guid> Add(Product product);
    }
}
