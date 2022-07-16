// Copyright (c) Microsoft.All Rights Reserved.Licensed under the MIT license.See License.txt in the project root for license information.

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
                var storageUrl = Environment.GetEnvironmentVariable("AZURE_STORAGE_URL");
                return new BlobServiceClient(new Uri(storageUrl ?? string.Empty));
            });

            services.AddTransient<IBlobStorageService, BlobStorageService>();

            return services;
        }
    }
}
