using System;

namespace Xam.NavigationView.Navigations.Extensions
{
    public interface INavResolver
    {
        object Resolve(Type viewType);
        object Resolve(Type viewType, object parameter);
    }

}
