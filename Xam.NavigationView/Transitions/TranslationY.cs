using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public class TranslationY : Transition
    {

        protected override void Apply(double value, VisualElement element)
        {
            element.TranslationY = value;
        }
    }
}
