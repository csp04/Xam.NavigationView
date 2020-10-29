using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public class ScaleY : Transition
    {

        protected override void Apply(double value, VisualElement element) => element.ScaleY = value;
    }
}
