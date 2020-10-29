using System;

namespace Xam.NavigationView
{
    public interface INavigationForViewResolver
    {
        object Resolve(Type type);
        object Resolve(Type type, params object[] parameters);
    }
}