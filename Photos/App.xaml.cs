using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Photos
{
    public partial class App : Application
    {
        NavigationPage np;
        PhotosPage pp;
        public App()
        {
            InitializeComponent();
            pp = new PhotosPage();
            np = new NavigationPage(pp);

            MainPage = np;
            //MainPage = new PhotosPage();
               
        }

        protected async override void OnStart()
        {
            // Handle when your app starts
            AzureRepo ar = new AzureRepo();
            await ar.getPictureBlob(pp);
            await ar.getCommentBlob(pp);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }



        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
