using Xamarin.Forms;

namespace X.NavView.Transitions
{
    public class Opacity : Transition
    {
        public Opacity() : base("_Opacity", VisualElement.OpacityProperty)
        {
        }
    }
}
