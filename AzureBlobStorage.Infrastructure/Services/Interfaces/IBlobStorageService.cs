// Copyright (c) Microsoft.All Rights Reserved.Licensed under the MIT license.See License.txt in the project root for license information.

using AzureBlobStorage.Infrastructure.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AzureBlobStorage.Infrastructure.Services.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadSingleBlob(
            Stream fileStream,
            string containerName,
            string blobName,
            CancellationToken cancellationToken
        );

        Task DeleteSingleBlob(
            string containerName,
            string blobName,
            CancellationToken cancellationToken
        );

        Task<Uri> GenerateSasToken(
            TimeSpan accessDuration,
            string containerName,
            string blobName,
            CancellationToken cancellationToken
        );

        Task<IList<BlobOptionsDto>> GetBlobs(
            string containerName,
            CancellationToken cancellationToken
        );
    }
}
