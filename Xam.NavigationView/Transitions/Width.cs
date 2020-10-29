using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public class Width : Transition
    {
        protected override void Apply(double value, VisualElement element) => element.WidthRequest = value;
    }
}
