using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;

using Foundation;
using UIKit;




namespace Photos.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            global::CarouselView.FormsPlugin.iOS.CarouselViewRenderer.Init();
            LoadApplication(new App());

          //  UINavigationBar.Appearance.BarTintColor = UIColor.White;
            return base.FinishedLaunching(app, options);
        }
    }
}
