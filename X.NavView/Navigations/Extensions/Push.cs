using System;
using Xamarin.Forms;

namespace Xam.NavigationView.Navigations.Extensions
{
    [ContentProperty("ViewType")]
    public sealed class Push : NavCommandExtension
    {
        public Type ViewType { get; set; }
        public object ViewParameter { get; set; }

        protected override Command CreateNavigationCommand(bool animated) => new Command(() => NavHelper.Push(ViewType, ViewParameter, animated));
    }
}