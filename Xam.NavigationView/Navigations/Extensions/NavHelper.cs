using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xam.NavigationView.Navigations.Extensions
{
    internal class NavHelper
    {
        public static INavigationForView GetNavigation() => ((HostView)Application.Current.MainPage).Navigation;

        public static Task PushAsync(Type viewType, object viewParameter, bool animated) => GetNavigation().PushAsync(CreateView(viewType, viewParameter), animated);

        public static Task PushModalAsync(Type viewType, object viewParameter, bool animated) => GetNavigation().PushModalAsync(CreateView(viewType, viewParameter), animated);

        private static ContentView CreateView(Type viewType, object viewParameter)
        {
            var resolver = NavigationForViewResolver.Instance;

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
