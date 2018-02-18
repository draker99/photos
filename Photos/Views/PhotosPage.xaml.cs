using System.Diagnostics;
using System.IO;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Photos
{
    public partial class PhotosPage : ContentPage
    {
        public ObservableCollection<UserPicture> pic { get; set; }
        public PhotosPage()
        {
            InitializeComponent();

            UserPicture p = new UserPicture();
            pic = new ObservableCollection<UserPicture>();


            // Handling my events
            b_addimage.Clicked += (sender, e) =>
            {
                this.GetPhoto();
            };
                      

            // Getting data back from native system
            MessagingCenter.Subscribe<byte[]>(this, "ImageSelected", (args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //Set the source of the image view with the byte array
                   
                    p.Picture = ImageSource.FromStream(() => new MemoryStream((byte[])args));
                    i_picture.Source = p.Picture;
                    pic.Add(new UserPicture(p.Picture));
                    lstView.ItemsSource = pic;
                    Debug.WriteLine("I am the Message Center" +  pic.Count);
                });
            });
         
        }

         void GetPhoto()   {
         
            Device.BeginInvokeOnMainThread(() =>
             {

                DependencyService.Get<CameraInterface>().BringUpPhotoGallery();
            
              });

        }

    /*    public async void Insert()   {

            AzureConnection conn = new AzureConnection();

            UserPicture picture = new UserPicture { AccountId = "Juhu!" };
            await conn.client.GetTable<UserPicture>().InsertAsync(picture);
            
        }
        */
    }
}
