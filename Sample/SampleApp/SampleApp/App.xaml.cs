using SampleApp.Views;
using System;
using Xam.NavigationView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApp
{
    public partial class App : Application
    {
        private static Lazy<HostView> HostView = new Lazy<HostView>(() => new HostView(new MainView()));
        public App()
        {
            InitializeComponent();

            MainPage = HostView.Value;
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
