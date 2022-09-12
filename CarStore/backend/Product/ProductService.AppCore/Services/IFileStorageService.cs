namespace ProductService.AppCore.Services
{
    public interface IFileStorageService
    {
        string? BuildFileUrl(string fileName);

        Uri? GetContainerSasUri();
    }
}
