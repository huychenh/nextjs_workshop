using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.AppContracts.Dtos
{
    public class SearchProductDto
    {
        public string? SearchText { get; set; }
        public string? CategoryName { get; set; }
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        public DateTime? Created { get; set; }
    }
    public class SearchProductRequestDto
    {
        public SearchProductDto? SearchProductModel { get; set; }
    }
}
