using System.Diagnostics;
using System.IO;
//using Microsoft.WindowsAzure.MobileServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml.Internals;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace Photos
{
    public partial class PhotosPage : ContentPage
    {
        public ObservableCollection<UserPicture> pic { get; set; }
        public UserPicture up = null;
        public ObservableCollection<PictureComment> comment { get; set; }
        public int counter = -1;
        public int current_pic_id = 0;
        public AzureRepo ar;

        public PhotosPage()
        {
            InitializeComponent();

            ar = new AzureRepo();


            UserPicture p = new UserPicture();
            pic = new ObservableCollection<UserPicture>();
            comment = new ObservableCollection<PictureComment>();
            comment.Where((comment) => comment.PictureId.Contains("0"));
            CarouselPics.ItemsSource = pic;
            lstView.ItemsSource = comment;
            layout_editor.IsVisible = false;


            Debug.WriteLine("Counter Settings: " + pic.Count.ToString());

            /*
            Image img = new Image
            {
                Source = "photo_begin"
            };
            */

        //    pic.Add(new UserPicture(img.Source, "0",null));

           
            // Getting data back from native system
            MessagingCenter.Subscribe<byte[]>(this, "ImageSelected", (args) =>
            {

                // async lambda expression because of blob storage
                 Device.BeginInvokeOnMainThread(async () =>
                {
                //Set the source of the image view with the byte array  
                counter++;

                p.Picture = ImageSource.FromStream(() => new MemoryStream((byte[])args));
                p.BytePicture = (byte[])args;
                up = new UserPicture(p.Picture, counter.ToString(), p.BytePicture);


                pic.Add(up);
                await ar.AddPicture(up);
                    Debug.WriteLine("Added Picture" + pic.Count.ToString());

                    CarouselPics.Position = counter;

                Debug.WriteLine("I am the Message Center" + counter.ToString());
                   
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


        async void comment_button(object sender, EventArgs e)
        {
            
            var text = editor.Text;
            PictureComment c = new PictureComment(text, current_pic_id.ToString(),comment.Count.ToString(), DateTime.Now.ToString());
            comment.Add(c);

            await ar.AddComment(c);

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
                Debug.WriteLine("Current Pic Id: " + current_pic_id.ToString());
                lstView.ItemsSource =comment.Where((comment) => comment.PictureId.Contains(e.NewValue.ToString()));
            }
            counter = pic.Count;
            Debug.WriteLine("Comment Counter: "  + comment.Count);    
        }

        /* deprecated */
        void tapped_listview(object sender, ItemTappedEventArgs e)
        {
          //  comment.Where((comment) => comment.PictureId.Contains(e.NewValue.ToString()));
            Debug.WriteLine("Tapped Listview"+ e.Item);
        }
    }
}
