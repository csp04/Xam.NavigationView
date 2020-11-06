
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xam.NavigationView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModalContainer : ContentView
    {
        internal ModalContainer()
        {
            InitializeComponent();
        }

        public void Add(View view) => container.Children.Add(view);

        public void Remove(View view) => container.Children.Remove(view);

        public void Insert(int index, View view) => container.Children.Insert(index, view);

        public int IndexOf(View view) => container.Children.IndexOf(view);

        #region ShadowOpacity Dependency Property
        public static readonly BindableProperty ShadowOpacityProperty =
            BindableProperty.Create("ShadowOpacity", typeof(double), typeof(ModalContainer), default(double), propertyChanged: OnShadowOpacityChanged);

        private static void OnShadowOpacityChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var container = bindable as ModalContainer;

            container.bgContainer.Opacity = (double)newValue;
        }

        public double ShadowOpacity
        {
            get
            {
                var value = (double)GetValue(ShadowOpacityProperty);

                if(value != bgContainer.Opacity)
                {
                    ShadowOpacity = value = bgContainer.Opacity;
                }

                return value;
            }
            set { SetValue(ShadowOpacityProperty, value); }
        }
        #endregion

    }
}