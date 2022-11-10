using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.AppCore.Services;

namespace ProductService.Api.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    public class FileStorageController : ControllerBase
    {
        private readonly ILogger<FileStorageController> _logger;
        private readonly IFileStorageService _fileStorageService;

        public FileStorageController(ILogger<FileStorageController> logger, IFileStorageService fileStorageService)
        {
            _logger = logger;
            _fileStorageService = fileStorageService;
        }

        [Authorize]
        [HttpGet("/api/v{version:apiVersion}/file-storage/container-url")]
        public ActionResult GetContainerUrl()
        {
            var uri = _fileStorageService.GetContainerSasUri();

            return Ok(uri?.AbsoluteUri);
        }
    }
}
