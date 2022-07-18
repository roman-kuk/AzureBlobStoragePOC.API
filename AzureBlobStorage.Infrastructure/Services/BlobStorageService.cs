// Copyright (c) Microsoft.All Rights Reserved.Licensed under the MIT license.See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using AzureBlobStorage.Infrastructure.Dtos;
using AzureBlobStorage.Infrastructure.Services.Interfaces;

namespace AzureBlobStorage.Infrastructure.Services
{
	public class BlobStorageService : IBlobStorageService
	{
		private readonly BlobServiceClient _blobServiceClient;

		public BlobStorageService(BlobServiceClient blobServiceClient)
		{
			_blobServiceClient = blobServiceClient;
		}

		public virtual async Task<string> UploadSingleBlob(
			Stream fileStream,
			string containerName,
			string blobName,
			CancellationToken cancellationToken
		)
		{
			var container = _blobServiceClient.GetBlobContainerClient(containerName);
			await container.CreateIfNotExistsAsync(PublicAccessType.None, cancellationToken: cancellationToken);

			await container.GetBlobClient(blobName).UploadAsync(fileStream, true, cancellationToken);

            return container.GetBlobClient(blobName).Name;
        }

        public virtual Task<Uri> GenerateSasToken(
			TimeSpan accessDuration,
			string containerName,
			string blobName,
			CancellationToken cancellationToken
		)
		{
			// Create a SAS token that's also valid for 7 days.
			var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
			var blobClient = blobContainerClient.GetBlobClient(blobName); // blob name

            var sasBuilder = new BlobSasBuilder()
			{
				BlobContainerName = blobClient.BlobContainerName,
				BlobName = blobClient.Name,
				Resource = "b", // b for blob, c for container
				StartsOn = DateTimeOffset.UtcNow.AddHours(-1),
				ExpiresOn = DateTimeOffset.UtcNow.Add(accessDuration),
			};

			sasBuilder.SetPermissions(BlobSasPermissions.Read); // read permissions
            // Add the SAS token to the container URI.

            var sasUri = blobClient.GenerateSasUri(sasBuilder);

			return Task.FromResult(sasUri);
		}

		public virtual async Task DeleteSingleBlob(
			string containerName,
			string blobName,
			CancellationToken cancellationToken
		)
		{
			var container = _blobServiceClient.GetBlobContainerClient(containerName);

			await container.DeleteBlobIfExistsAsync(blobName, cancellationToken: cancellationToken);
		}

        public virtual Task<IList<BlobOptionsDto>> GetBlobs(
            string containerName,
            CancellationToken cancellationToken
        )
        {
            var container = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobs = AggregateBlobItems(container, cancellationToken).Select(x => new BlobOptionsDto()
            {
                FileName = x.Name
            }).ToList();

            return Task.FromResult<IList<BlobOptionsDto>>(blobs);
        }

        private static IEnumerable<BlobItem> AggregateBlobItems(BlobContainerClient container, CancellationToken cancellationToken)
        {
            var result = new List<BlobItem>();

            using var enumerator = container.GetBlobs(BlobTraits.None, BlobStates.None, null, cancellationToken)
                .AsPages()
                .GetEnumerator();

            do
            {
                if (enumerator.Current != null) result.AddRange(enumerator.Current.Values);
            } while (enumerator.MoveNext());

            return result;
        }
	}
}
