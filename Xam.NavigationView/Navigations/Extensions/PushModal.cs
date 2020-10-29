using Xamarin.Forms;

namespace Xam.NavigationView.Navigations.Extensions
{
    public sealed class PushModal : PushBaseCommandExtension
    {
        protected override Command CreateNavigationCommand(bool animated)
        {
            return new Command(() => NavHelper.PushModalAsync(ViewType, ViewParameter, animated));
        }
    }
}
