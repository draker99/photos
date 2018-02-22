using System.Diagnostics;
using System.IO;
using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml.Internals;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;

namespace Photos
{
    public partial class PhotosPage : ContentPage
    {
        public ObservableCollection<UserPicture> pic { get; set; }
        public UserPicture up = null;
        public ObservableCollection<PictureComment> comment { get; set; }
        public int counter = -1;
        public int current_pic_id = 0;
        public PhotosPage()
        {
            InitializeComponent();
         
            UserPicture p = new UserPicture();
            pic = new ObservableCollection<UserPicture>();
            comment = new ObservableCollection<PictureComment>();
            comment.Where((comment) => comment.PictureId.Contains("0"));
            CarouselPics.ItemsSource = pic;
            lstView.ItemsSource = comment;
            layout_editor.IsVisible = false;


            Image img = new Image
            {
                Source = "photo_begin"
            };

            pic.Add(new UserPicture(img.Source, "0"));


           
            // Getting data back from native system
            MessagingCenter.Subscribe<byte[]>(this, "ImageSelected", (args) =>
            {
                
                Device.BeginInvokeOnMainThread(() =>
                {
                //Set the source of the image view with the byte array

                p.Picture = ImageSource.FromStream(() => new MemoryStream((byte[])args));
                    pic.Add(new UserPicture(p.Picture,counter.ToString()));
                    counter++;
                    CarouselPics.Position = counter;

                Debug.WriteLine("I am the Message Center" +  pic.Count);
                   
                });
            }); 
        }

        void GetPhoto(object sender, EventArgs e)   {
            if (counter == -1)
            {
                pic.Clear();
            }

            Device.BeginInvokeOnMainThread(() =>
             {
                DependencyService.Get<CameraInterface>().BringUpPhotoGallery();            
              });
        }

        void AddComment(object sender, EventArgs e)
        {
            layout_editor.IsVisible = true;
            CarouselPics.IsVisible = false;
            editor.Focus();
        }


        void comment_button(object sender, EventArgs e)
        {
            var text = editor.Text;
            comment.Add(new PictureComment(text, current_pic_id.ToString()));
            lstView.ItemsSource = comment.Where((comment) => comment.PictureId.Contains(current_pic_id.ToString()));
            layout_editor.IsVisible = false;
            CarouselPics.IsVisible = true;
            editor.Text = "";
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
                lstView.ItemsSource =comment.Where((comment) => comment.PictureId.Contains(e.NewValue.ToString()));


            }
            Debug.WriteLine("POS_SEL: "  + comment.Count);    
        }

        /* deprecated */
        void tapped_listview(object sender, ItemTappedEventArgs e)
        {
          //  comment.Where((comment) => comment.PictureId.Contains(e.NewValue.ToString()));
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
