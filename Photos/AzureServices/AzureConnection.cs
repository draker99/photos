using System;
using Microsoft.WindowsAzure.MobileServices;


namespace Photos
{
    public class AzureConnection
    {
        public MobileServiceClient client;

        public AzureConnection()
        {
            this.client = new MobileServiceClient("https://cpiphotos.azurewebsites.net");
        }


        public void Initialize()    {
            
        }

    }
}
