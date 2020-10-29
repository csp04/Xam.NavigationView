using Xamarin.Forms;

namespace Xam.NavigationView.Navigations.Extensions
{
    public sealed class Pop : NavCommandExtension
    {
        protected override Command CreateNavigationCommand(bool animated)
        {
            return new Command(() => NavHelper.GetNavigation().PopAsync(animated));
        }
    }
}
