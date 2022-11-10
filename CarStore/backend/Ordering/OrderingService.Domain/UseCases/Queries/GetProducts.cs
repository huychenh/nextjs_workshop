using CarStore.AppContracts.Dtos;
using FluentValidation;
using MediatR;
using N8T.Core.Domain;

namespace ProductService.AppCore.UseCases.Queries
{
    public class GetProducts
    {
        public class Query : SearchProductRequestDto, IQuery<IEnumerable<ProductDto>>
        {
            internal class Validator : AbstractValidator<Query>
            {
                public Validator()
                {
                }
            }

            //internal class Handler : IRequestHandler<Query, ResultModel<IEnumerable<ProductDto>>>
            //{
            //    private readonly IRepository _repository;

            //    public Handler(IRepository repository)
            //    {
            //        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            //    }

            //    public async Task<ResultModel<IEnumerable<ProductDto>>> Handle(Query request, CancellationToken cancellationToken)
            //    {
            //        if (request == null)
            //        {
            //            throw new ArgumentNullException(nameof(request));
            //        }

            //        IEnumerable<ProductDto> products;
            //        if (request.SearchProductModel.Page != null && request.SearchProductModel.PageSize != null)
            //        {
            //            products = await _repository.GetWithPagination(request.SearchProductModel);                        
            //        }
            //        else
            //        {
            //            products = await _repository.Get(request.SearchProductModel);
            //        }
                    
            //        return ResultModel<IEnumerable<ProductDto>>.Create(products);
            //    }
            //}
        }
    }
}
