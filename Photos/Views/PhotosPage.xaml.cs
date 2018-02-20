using System.Diagnostics;
using System.IO;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml.Internals;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;


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
            CarouselPics.ItemsSource = pic;


            //lstView.ItemsSource = pic;

            // Getting data back from native system
            MessagingCenter.Subscribe<byte[]>(this, "ImageSelected", (args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                //Set the source of the image view with the byte array

                p.Picture = ImageSource.FromStream(() => new MemoryStream((byte[])args));
                pic.Add(new UserPicture(p.Picture));
                Debug.WriteLine("I am the Message Center" +  pic.Count);
                   
                });
            }); 
        }

        void GetPhoto(object sender, EventArgs e)   {
         
            Device.BeginInvokeOnMainThread(() =>
             {

                DependencyService.Get<CameraInterface>().BringUpPhotoGallery();
            
              });
        }

        void AddComment(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CommentPage());
        }


        void tapped_listview(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                UserPicture up = (UserPicture)e.Item;
              //  i_picture.Source = up.Picture;
            }
            Debug.WriteLine("Tapped Listview"+ e.Item);
        }

    /*    public async void Insert()   {

            AzureConnection conn = new AzureConnection();

            UserPicture picture = new UserPicture { AccountId = "Juhu!" };
            await conn.client.GetTable<UserPicture>().InsertAsync(picture);
            
        }
        */
    }
}
