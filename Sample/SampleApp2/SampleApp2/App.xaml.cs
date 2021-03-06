﻿using SampleApp2.Views;
using System;
using X.NavView;
using Xamarin.Forms;

namespace SampleApp2
{
    public partial class App : Application
    {

        private static readonly Lazy<HostView> HostView = new Lazy<HostView>(() => new HostView(new MainView()));

        public App()
        {
            InitializeComponent();

            if (MainPage == null)
            {
                MainPage = HostView.Value;

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
