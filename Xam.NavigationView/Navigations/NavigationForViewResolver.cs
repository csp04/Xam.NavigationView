using System;

namespace Xam.NavigationView
{
    public class NavigationForViewResolver : INavigationForViewResolver
    {
        private static INavigationForViewResolver _resolver;
        private static readonly INavigationForViewResolver _defaultResolver = new NavigationForViewResolver();

        private NavigationForViewResolver()
        {

        }

        public static INavigationForViewResolver Instance
        {
            get
            {
                if (_resolver != null)
                {
                    return _resolver;
                }

                return _defaultResolver;
            }
        }

        public static void SetResolver(INavigationForViewResolver resolver) => _resolver = resolver;

        object INavigationForViewResolver.Resolve(Type type) => Activator.CreateInstance(type);

        object INavigationForViewResolver.Resolve(Type type, params object[] parameters) => Activator.CreateInstance(type, parameters);
    }
}