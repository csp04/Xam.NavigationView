using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public class ScaleX : Transition
    {

        protected override void Apply(double value, VisualElement element) => element.ScaleX = value;
    }
}
