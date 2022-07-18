using AzureBlobStorage.Infrastructure.Dtos;
using AzureBlobStoragePOC.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AzureBlobStoragePOC.API.Controllers
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileManagementService _fileManagementService;
        private const int MaxFileSize = 114857600;

        public FilesController(IFileManagementService fileManagementService)
        {
            _fileManagementService = fileManagementService;
        }

        [HttpPost]
        [RequestSizeLimit(MaxFileSize)]
        public async Task<string> UploadFile(
            IFormFile file,
            CancellationToken cancellationToken
        )
        {
            await using var fileStream = file.OpenReadStream();
            return await _fileManagementService.UploadAsync(fileStream, new BlobOptionsDto()
            {
                CreatedAt = DateTime.UtcNow,
                FileName = Path.GetFileNameWithoutExtension(file.FileName),
                FileExtension = Path.GetExtension(file.FileName)
            }, cancellationToken);
        }

        [HttpGet("{name}/download-url")]
        public async Task<string> GetFileDownloadUrl(
            [FromRoute] string name,
            CancellationToken cancellationToken
        )
        {
            return await _fileManagementService.GetFileDownloadUrl(name, cancellationToken);
        }

        [HttpGet]
        public async Task<IList<string>> GetFiles(
            CancellationToken cancellationToken
        )
        {
            return await _fileManagementService.GetAllNames(cancellationToken);
        }

        [HttpDelete("{name}")]
        public async Task DeleteFile(
            [FromRoute] string name,
            CancellationToken cancellationToken
        )
        {
            await _fileManagementService.DeleteAsync(name, cancellationToken);
        }
    }
}
