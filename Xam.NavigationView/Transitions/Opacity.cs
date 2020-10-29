using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public class Opacity : Transition
    {
        protected override void Apply(double value, VisualElement element) => element.Opacity = value;
    }

    public class ModalContainerOpacity : Transition<ModalContainer>
    {
        protected override void Apply(double value, ModalContainer element) => element.Opacity = value;
    }
}
