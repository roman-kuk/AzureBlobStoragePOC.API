// Copyright (c) Microsoft.All Rights Reserved.Licensed under the MIT license.See License.txt in the project root for license information.

using Azure.Storage;
using Azure.Storage.Blobs;
using AzureBlobStorage.Infrastructure.Services;
using AzureBlobStorage.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AzureBlobStorage.Infrastructure
{
    public static class InfrastructureDependencyInjectionContainer
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient(_ =>
            {
                var accountName = Environment.GetEnvironmentVariable("AZURE_STORAGE_URL");
                var accessKey = Environment.GetEnvironmentVariable("AZURE_STORAGE_ACCESS_KEY");
                var client = new BlobServiceClient(new Uri($"https://{accountName}.blob.core.windows.net"),
                    new StorageSharedKeyCredential(accountName, accessKey));
                return client;
            });

            services.AddTransient<IBlobStorageService, BlobStorageService>();

            return services;
        }
    }
}
