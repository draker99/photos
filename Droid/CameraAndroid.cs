using System;
using Android.App;
using Android.Content;
using Android.Provider;
using Xamarin.Forms;


[assembly: Dependency(typeof(Photos.Droid.CameraAndroid))]
namespace Photos.Droid
{
    public class CameraAndroid : CameraInterface
    {
        public CameraAndroid()
        {
        }

        public void BringUpPhotoGallery()
        {
            var imageIntent = new Intent();
            imageIntent.SetType("image/*");
            imageIntent.SetAction(Intent.ActionGetContent);

            ((Activity)Forms.Context).StartActivityForResult(Intent.CreateChooser(imageIntent, "Select photo"), 1);
        }
    }
}
