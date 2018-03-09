using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;
using System.Net.Http;
using System;

namespace Photos
{
    public class AzureConnection
    {
        private static readonly Random Random = new Random();
        private readonly CloudStorageAccount _storageAccount;
        private readonly CloudBlobClient _blobClient;


        public AzureConnection(CloudStorageAccount storageAccount)
        {
            _storageAccount = storageAccount;
            _blobClient = _storageAccount.CreateCloudBlobClient();
        }
    }
      
}
