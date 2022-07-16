// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the MIT license.  See License.txt in the project root for license information.

using System;

namespace AzureBlobStorage.Infrastructure.Dtos
{
	public class BlobOptionsDto
	{
		public string FileName { get; set; }
		public string FileExtension { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
