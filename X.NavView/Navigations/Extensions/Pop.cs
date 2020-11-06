using Xamarin.Forms;

namespace Xam.NavigationView.Navigations.Extensions
{
    public sealed class Pop : NavCommandExtension
    {
        protected override Command CreateNavigationCommand(bool animated) => new Command(() => NavHelper.Navigation.Pop(animated));
    }
}