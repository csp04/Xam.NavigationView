using SampleApp2.Views;
using System;
using X.NavView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApp2
{
    public partial class App : Application
    {
       
        public App()
        {
            InitializeComponent();

            if(MainPage == null)
            {
                MainPage = new HostView(new MainView());

                Sharpnado.Shades.Initializer.Initialize(loggerEnable: false);
            }

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
