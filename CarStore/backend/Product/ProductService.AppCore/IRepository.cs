using N8T.Core.Domain;
using ProductService.AppCore.Core;
using ProductService.AppCore.UseCases.Queries;
using ProductService.Shared.DTO;

namespace ProductService.AppCore
{
    public interface IRepository
    {
        Task<ListResultModel<ProductDto>> GetWithPagination(GetProducts query);

        Task<ProductDto?> GetById(Guid id);

        Task<Guid> Add(Product product);
    }
}
