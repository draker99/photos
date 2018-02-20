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
        public UserPicture up = null;
        public int counter = 0;
        public int current_pic_id = 0;
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
                    pic.Add(new UserPicture(p.Picture,counter.ToString()));
                    counter++;


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
            Navigation.PushAsync(new CommentPage(current_pic_id,pic[current_pic_id]));
        }

        void pos_sel(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e) 
        {

            /*   if(CarouselPics.ItemsSource != null) {
                   up = (UserPicture)CarouselPics.ItemsSource
               }
   */
            if (pic.Count != 0)
            {
                current_pic_id = e.NewValue;
                l_counter.Text = counter.ToString() + "/" + e.NewValue.ToString() + "/" + pic[e.NewValue].Id;
            }
            Debug.WriteLine("something happend" );    
        }

        /* deprecated */
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
