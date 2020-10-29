using Xamarin.Forms;

namespace Xam.NavigationView.Navigations.Extensions
{
    public sealed class PopModal : NavCommandExtension
    {
        protected override Command CreateNavigationCommand(bool animated)
        {
            return new Command(() => NavHelper.GetNavigation().PopModalAsync(animated));
        }
    }
}
