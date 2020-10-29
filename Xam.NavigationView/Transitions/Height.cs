using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public class Height : Transition
    {
        protected override void Apply(double value, VisualElement element)
        {
            element.HeightRequest = value;
        }
    }
}
