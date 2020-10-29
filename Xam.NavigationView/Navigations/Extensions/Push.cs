using Xamarin.Forms;

namespace Xam.NavigationView.Navigations.Extensions
{
    public sealed class Push : PushBaseCommandExtension
    {
        protected override Command CreateNavigationCommand(bool animated) => new Command(() => NavHelper.PushAsync(ViewType, ViewParameter, animated));
    }
}
