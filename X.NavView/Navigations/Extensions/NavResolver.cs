using System;

namespace Xam.NavigationView.Navigations.Extensions
{
    public class NavResolver : INavResolver
    {
        private static INavResolver _resolver;
        private static readonly INavResolver _defaultResolver = new NavResolver();

        private NavResolver()
        {
        }

        object INavResolver.Resolve(Type viewType) => Activator.CreateInstance(viewType);
        object INavResolver.Resolve(Type viewType, object parameter) => Activator.CreateInstance(viewType, parameter);

        public static INavResolver Instance
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

        public static void SetResolver(INavResolver navResolver) => _resolver = navResolver;
    }

}
