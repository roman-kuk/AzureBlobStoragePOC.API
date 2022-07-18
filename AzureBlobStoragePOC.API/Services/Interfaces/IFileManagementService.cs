using AzureBlobStorage.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AzureBlobStoragePOC.API.Services.Interfaces
{
    public interface IFileManagementService
    {
        Task<string> UploadAsync(Stream fileStream, BlobOptionsDto blobOptionsDto,
            CancellationToken cancellationToken);

        Task DeleteAsync(string name, CancellationToken cancellationToken);

        Task<string> GetFileDownloadUrl(
            string name,
            CancellationToken cancellationToken
        );

        Task<IList<string>> GetAllNames(CancellationToken cancellationToken);
    }
}
