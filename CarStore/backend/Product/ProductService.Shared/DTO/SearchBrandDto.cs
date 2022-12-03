namespace ProductService.Shared.DTO
{
    public class SearchBrandDto
    {
        public string? SearchText { get; set; }
    }

    public class SearchBrandRequestDto
    {
        public SearchBrandDto SearchBrandModel { get; set; }
    }
}
