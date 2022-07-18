using AzureBlobStorage.Infrastructure.Dtos;
using AzureBlobStorage.Infrastructure.Services.Interfaces;
using AzureBlobStoragePOC.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AzureBlobStoragePOC.API.Services
{
    public class FileManagementService : IFileManagementService
    {
        private readonly IBlobStorageService _blobStorageService;
        private const string ContainerName = "shared";
        private static readonly TimeSpan AccessDuration = new(0, 1, 0);

        public FileManagementService(IBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public async Task<string> UploadAsync(Stream fileStream, BlobOptionsDto blobOptionsDto, CancellationToken cancellationToken)
        {
            return await _blobStorageService.UploadSingleBlob(
                fileStream,
                ContainerName,
                $"{blobOptionsDto.FileName}_{blobOptionsDto.CreatedAt:dd_MM_yyyy-hh_mm_ss.fff}.{blobOptionsDto.FileExtension.Replace(".", "")}",
                cancellationToken
            );
        }

        public async Task DeleteAsync(string name, CancellationToken cancellationToken)
        {
            await _blobStorageService.DeleteSingleBlob(
                ContainerName,
                name,
                cancellationToken
            );
        }

        public async Task<string> GetFileDownloadUrl(
            string name,
            CancellationToken cancellationToken
        )
        {
            return (await _blobStorageService.GenerateSasToken(
                AccessDuration,
                ContainerName,
                name,
                cancellationToken)).AbsoluteUri;
        }

        public async Task<IList<string>> GetAllNames(CancellationToken cancellationToken)
        {
            return (await _blobStorageService.GetBlobs(ContainerName, cancellationToken))
                .Select(x => x.FileName)
                .ToList();
        }
    }
}
