using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductService.AppCore.Services;

namespace ProductService.Infrastructure.Services
{
    public class FileStorageOptions
    {
        public string ConnectionString { get; set; }

        public string ImageContainerName { get; set; }
    }

    public class FileStorageService : IFileStorageService
    {
        private static BlobContainerClient? _blobContainerClient;
        private static readonly object _lock = new();

        private readonly ILogger<FileStorageService> _logger;
        private readonly FileStorageOptions _options;

        public FileStorageService(ILogger<FileStorageService> logger, IOptions<FileStorageOptions> optionAccessor)
        {
            _logger = logger;
            _options = optionAccessor.Value;
        }

        protected BlobContainerClient? BlobContainerClient
        {
            get
            {
                if (_blobContainerClient == null)
                {
                    lock (_lock)
                    {
                        if (_blobContainerClient == null)
                        {
                            var blobServiceClient = new BlobServiceClient(_options.ConnectionString);
                            _blobContainerClient = CreateBlobContainerAsync(blobServiceClient, _options.ImageContainerName).Result;
                        }
                    }
                }

                return _blobContainerClient;
            }
        }

        public string? BuildFileUrl(string fileName)
        {
            if (BlobContainerClient == null)
            {
                _logger.LogError("BlobContainerClient has not been initialized.");
                return null;
            }

            if (string.IsNullOrEmpty(fileName))
            {
                return fileName;
            }

            var blob = BlobContainerClient.GetBlobClient(fileName);
            return blob.Uri.AbsoluteUri;
        }

        public Uri? GetContainerSasUri()
        {
            if (BlobContainerClient == null)
            {
                _logger.LogError("BlobContainerClient has not been initialized.");
                return null;
            }

            if (!BlobContainerClient.CanGenerateSasUri)
            {
                _logger.LogError(@"BlobContainerClient must be authorized with Shared Key credentials to create a service SAS.");
                return null;
            }

            // Create a SAS token that's valid for one hour.
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = BlobContainerClient.Name,
                Resource = "c",
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1),
            };

            sasBuilder.SetPermissions(BlobContainerSasPermissions.Write | BlobContainerSasPermissions.Read);
            Uri sasUri = BlobContainerClient.GenerateSasUri(sasBuilder);
            _logger.LogInformation("SAS URI for blob container is: {0}", sasUri);

            return sasUri;
        }

        private async Task<BlobContainerClient?> CreateBlobContainerAsync(BlobServiceClient blobServiceClient, string containerName)
        {
            try
            {
                var container = blobServiceClient.GetBlobContainerClient(containerName);
                await container.CreateIfNotExistsAsync(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
                return container;
            }
            catch (RequestFailedException e)
            {
                _logger.LogError("HTTP error code {0}: {1}", e.Status, e.ErrorCode);
            }

            return null;
        }
    }
}
