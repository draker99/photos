using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;

namespace Photos
{
    public partial class PhotosPage : ContentPage
    {
        public PhotosPage()
        {
            InitializeComponent();

            this.Insert();

         
        }

        public async void Insert()   {

            AzureConnection conn = new AzureConnection();

            UserPicture picture = new UserPicture { AccountId = "Juhu!" };
            await conn.client.GetTable<UserPicture>().InsertAsync(picture);
            
        }
    }
}
