using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public class TranslationX : Transition
    {

        protected override void Apply(double value, VisualElement element) => element.TranslationX = value;
    }
}
