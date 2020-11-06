using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xam.NavigationView.Navigations.Extensions
{
    public abstract class NavCommandExtension : IMarkupExtension<Command>
    {
        public bool Animated { get; set; } = true;

        public Command ProvideValue(IServiceProvider serviceProvider) => CreateNavigationCommand(Animated);

        protected abstract Command CreateNavigationCommand(bool animated);

        object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) => (this as IMarkupExtension<Command>).ProvideValue(serviceProvider);
    }
}