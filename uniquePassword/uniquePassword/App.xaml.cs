using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace uniquePassword
{
    public partial class App : Application
    {
        public App()
        {
            //initialize live reload.
#if debug
            livereload.init();
#endif

            InitializeComponent();

            MainPage = new GeneratePage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
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
