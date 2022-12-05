using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HelpMe.Views.Navigation;
using HelpMe.Views.Register;


namespace HelpMe
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
            MainPage = new NavigationPage(new CrearCuenta());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
