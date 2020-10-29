using System;
using Xamarin.Forms;

namespace Xam.NavigationView.Navigations.Extensions
{
    [ContentProperty("ViewType")]
    public abstract class PushBaseCommandExtension : NavCommandExtension
    {
        public Type ViewType { get; set; }
        public object ViewParameter { get; set; }
    }
}
