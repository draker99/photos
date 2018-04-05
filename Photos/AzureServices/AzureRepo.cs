using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Xamarin.Forms;

namespace Photos
{
    public class AzureRepo
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly AzureConnection _blobService;

        UserPicture p = new UserPicture();
        public ObservableCollection<UserPicture> pic { get; set; }

        public AzureRepo()
        {
            _storageAccount =
                CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=xamarinphoto;AccountKey=VKK/awZJuzJ2TrCQszdFD0ntrphzeVapPxQY8d+UjTQ/ZSWef3KHoAh65klz5b6vtMyHiLaUhtVVIY8lMwh8vQ==;EndpointSuffix=core.windows.net");
            _blobService = new AzureConnection(_storageAccount);
        }



        public async Task AddPicture(UserPicture pic)
        {
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("xamarinphotocontainer");

            await container.CreateIfNotExistsAsync();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference("photos_blob" + pic.Id);

            blockBlob.Metadata.Add("Id", pic.Id);
            Debug.WriteLine("Length of picture: " + pic.BytePicture.Length +  " - " + pic.Id);
            await blockBlob.UploadFromByteArrayAsync(pic.BytePicture, 0, pic.BytePicture.Length);
        }

        public async Task AddComment(PictureComment com)
        {
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("xamarincommentcontainer");

            await container.CreateIfNotExistsAsync();

            CloudBlockBlob blockBlob = container.GetBlockBlobReference("comments_blob" + com.Id);

            blockBlob.Metadata.Add("Id", com.Id);
            blockBlob.Metadata.Add("PictureId", com.PictureId);
            blockBlob.Metadata.Add("PictureDate", com.CurrentDate);
            await blockBlob.UploadTextAsync(com.Comment);
        }

        public async Task getPictureBlob(PhotosPage page)  {
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("xamarinphotocontainer");

            await container.CreateIfNotExistsAsync();

            BlobContinuationToken token = null;

            var s = await container.ListBlobsSegmentedAsync(token);

            UserPicture up;


            foreach(IListBlobItem item in s.Results) {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                   
                    var Id = "0";
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    byte[] blobBytes = new byte[blob.Properties.Length];
                    Debug.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);
                    await blob.DownloadToByteArrayAsync(blobBytes,0);
                    foreach(var metadataItem in blob.Metadata)  {
                        if(metadataItem.Key == "Id") {
                            Id = metadataItem.Value.ToString();
                        }
                    }

                    p.BytePicture = blobBytes;
                    p.Picture = ImageSource.FromStream(() => new MemoryStream(blobBytes));

                      up = new UserPicture(p.Picture, Id, p.BytePicture);
                      page.pic.Add(up);
                }
            }
        }


        public async Task getCommentBlob(PhotosPage page)
        {
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();

            CloudBlobContainer container = blobClient.GetContainerReference("xamarincommentcontainer");

            await container.CreateIfNotExistsAsync();

            BlobContinuationToken token = null;

            var s = await container.ListBlobsSegmentedAsync(token);

            PictureComment pc;

            foreach (IListBlobItem item in s.Results)
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {

                    var PictureId = "";
                    var Id = "";
                    var PictureDate = "";
                    CloudBlockBlob blob = (CloudBlockBlob)item;
                    //byte[] blobBytes = new byte[blob.Properties.Length];
                    Debug.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);
                    var comment = await blob.DownloadTextAsync();
                    foreach (var metadataItem in blob.Metadata)
                    {
                        if (metadataItem.Key == "PictureId")
                        {
                            PictureId = metadataItem.Value.ToString();
                        }
                        else if (metadataItem.Key == "Id")
                        {
                            Id = metadataItem.Value.ToString();
                        }
                        else if (metadataItem.Key == "PictureDate")
                        {
                            PictureDate = metadataItem.Value.ToString();
                        }
                    }
                    Debug.WriteLine("Block blob of length {0}: {1}", PictureId, comment);
                  
                    pc = new PictureComment(comment,PictureId,Id,PictureDate);
                    page.comment.Add(pc);   
                }
            }
        }
       

    }
}

