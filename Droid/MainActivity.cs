﻿using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Database;
using Android.Graphics;
using System.IO;
using Xamarin.Forms;
using Android.Provider;

namespace Photos.Droid
{
    [Activity(Label = "Photos.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }
   


     protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            //Since we set the request code to 1 for both the camera and photo gallery, that's what we need to check for
            if (requestCode == 1)
            {
                if (resultCode == Result.Ok)
                {
                    if (data.Data != null)
                    {
                        //Grab the Uri which is holding the path to the image
                        Android.Net.Uri uri = data.Data;

                        //Read the meta data of the image to determine what orientation the image should be in
                        int orientation = getOrientation(uri);

                        //Stat a background task so we can do all the bitmap stuff off the UI thread
                        BitmapWorkerTask task = new BitmapWorkerTask(this.ContentResolver, uri);
                        task.Execute(orientation);
                    }
                }
            }
        }

        public int getOrientation(Android.Net.Uri photoUri)
        {
            ICursor cursor = Application.ApplicationContext.ContentResolver.Query(photoUri, new String[] { MediaStore.Images.ImageColumns.Orientation }, null, null, null);

            if (cursor.Count != 1)
            {
                return -1;
            }

            cursor.MoveToFirst();
            return cursor.GetInt(0);
        }

    }



    public class BitmapWorkerTask : AsyncTask<int, int, Bitmap>
    {
        private Android.Net.Uri uriReference;
        private int data = 0;
        private ContentResolver resolver;

        public BitmapWorkerTask(ContentResolver cr, Android.Net.Uri uri)
        {
            uriReference = uri;
            resolver = cr;
        }

        // Decode image in background.
        protected override Bitmap RunInBackground(params int[] p)
        {
            //This will be the orientation that was passed in from above (task.Execute(orientation);)
            data = p[0];

            Bitmap mBitmap = Android.Provider.MediaStore.Images.Media.GetBitmap(resolver, uriReference);
            Bitmap myBitmap = null;

            if (mBitmap != null)
            {
                //In order to rotate the image we create a Matrix object, rotate if the image is not already in it's correct orientation
                Matrix matrix = new Matrix();
                if (data != 0)
                {
                    matrix.PreRotate(data);
                }

                myBitmap = Bitmap.CreateBitmap(mBitmap, 0, 0, mBitmap.Width, mBitmap.Height, matrix, true);
                return myBitmap;
            }

            return null;
        }

        //Called when the RunInBackground method has finished
        protected override void OnPostExecute(Bitmap bitmap)
        {
            if (bitmap != null)
            {
                MemoryStream stream = new MemoryStream();

                //Compressing by 50%, feel free to change if file size is not a factor
                bitmap.Compress(Bitmap.CompressFormat.Jpeg, 50, stream);
                byte[] bitmapData = stream.ToArray();

                //Send image byte array back to UI
                MessagingCenter.Send<byte[]>(bitmapData, "ImageSelected");

                //clean up bitmaps so the app doesn't crash from using up too much memory
                bitmap.Recycle();
                GC.Collect();
            }
        }
    }
}
