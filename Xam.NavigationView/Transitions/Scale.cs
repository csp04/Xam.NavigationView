using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public class Scale : Transition
    {

        protected override void Apply(double value, VisualElement element) => element.Scale = value;
    }
}
