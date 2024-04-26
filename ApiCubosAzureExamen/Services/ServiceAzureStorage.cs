using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace ApiCubosAzureExamen.Services
{
    public class ServiceAzureStorage
    {
        private BlobServiceClient client;


        public ServiceAzureStorage(BlobServiceClient blobService)
        {
            this.client = blobService;
        }

        public string GetContainerUrl(string containerName)
        {
            BlobContainerClient containerClient = client.GetBlobContainerClient(containerName);
            return
                containerClient.Uri.OriginalString;
        }
    }
}
