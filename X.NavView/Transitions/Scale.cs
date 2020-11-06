using Xamarin.Forms;

namespace X.NavView.Transitions
{
    public class Scale : Transition
    {
        public Scale() : base("_Scale", VisualElement.ScaleProperty)
        {
        }
    }
}