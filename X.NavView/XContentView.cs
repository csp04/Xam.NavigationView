using X.NavView.Navigations;
using Xamarin.Forms;

namespace X.NavView
{
    public class XContentView : ContentView, IXViewController
    {
        public XContentView() => BackgroundColor = Color.White;

        public INavigator Navigator { get; } = new NavigatorImpl();

        protected virtual void OnAppearing()
        {
        }

        protected virtual void OnDisappearing()
        {
        }

        void IXViewController.SendAppearing() => OnAppearing();

        void IXViewController.SendDisappearing() => OnDisappearing();
    }
}