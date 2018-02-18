using System;
using AVFoundation;
using Foundation;
using UIKit;
using Xamarin.Forms;
using CoreGraphics;

[assembly: Dependency(typeof(Photos.iOS.CameriaIOS))]
namespace Photos.iOS
{
    public class CameriaIOS : CameraInterface
    {
        public CameriaIOS()
        {
        }

        public void BringUpPhotoGallery()
        {
            var imagePicker = new UIImagePickerController { SourceType = UIImagePickerControllerSourceType.PhotoLibrary, MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary) };
            imagePicker.AllowsEditing = true;

            //Make sure we have the root view controller which will launch the photo gallery 
            var window = UIApplication.SharedApplication.KeyWindow;
            var vc = window.RootViewController;
            while (vc.PresentedViewController != null)
            {
                vc = vc.PresentedViewController;
            }

            //Show the image gallery
            vc.PresentViewController(imagePicker, true, null);

            //call back for when a picture is selected and finished editing
            imagePicker.FinishedPickingMedia += (sender, e) =>
            {
                UIImage originalImage = e.Info[UIImagePickerController.EditedImage] as UIImage;
                if (originalImage != null)
                {
                    //Got the image now, convert it to byte array to send back up to the forms project
                    var pngImage = originalImage.AsPNG();
                    byte[] myByteArray = new byte[pngImage.Length];
                    System.Runtime.InteropServices.Marshal.Copy(pngImage.Bytes, myByteArray, 0, Convert.ToInt32(pngImage.Length));

                    MessagingCenter.Send<byte[]>(myByteArray, "ImageSelected");
                }

                //Close the image gallery on the UI thread
                Device.BeginInvokeOnMainThread(() =>
                {
                    vc.DismissViewController(true, null);
                });
            };

            //Cancel button callback from the image gallery
            imagePicker.Canceled += (sender, e) => vc.DismissViewController(true, null);
        }


    }
}
