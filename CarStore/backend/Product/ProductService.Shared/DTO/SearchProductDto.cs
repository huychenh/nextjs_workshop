using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Shared.DTO
{
    public class SearchProductDto
    {
        public string? SearchText { get; set; }
        public string? CategoryName { get; set; }
        public string? Brand { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        public bool? LatestNews { get; set; }
        public bool? LowestPrice { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class SearchProductRequestDto
    {
        public SearchProductDto? SearchProductModel { get; set; }
    }
}
