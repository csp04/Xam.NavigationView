using System;
using System.Threading.Tasks;
using X.NavView;
using X.NavView.Navigations;
using Xamarin.Forms;

namespace Xam.NavigationView.Navigations.Extensions
{
    internal class NavHelper
    {
        public static INavigator Navigation => ((HostView)Application.Current.MainPage).Navigator;

        public static Task Push(Type viewType, object viewParameter, bool animated) => Navigation.Push(CreateView(viewType, viewParameter), animated);

        public static Task Pop(bool animated) => Navigation.Pop(animated);

        private static ContentView CreateView(Type viewType, object viewParameter)
        {
            var resolver = NavResolver.Instance;

            if (viewParameter != null)
            {
                return resolver.Resolve(viewType, viewParameter) as ContentView;
            }
            else
            {
                return resolver.Resolve(viewType) as ContentView;
            }
        }
    }
}