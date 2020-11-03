using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public class SlideFromRightEnter : ScreenAwareTransition
    {
        protected override void OnBeforeAnimating()
        {
            Start = GetHostWidth();
            End = 0;
            Easing = EasingMode.CubicOut;
        }

        protected override void Apply(double value, VisualElement view) => view.TranslationX = value;
    }

    public class SlideToRightExit : ScreenAwareTransition
    {
        protected override void OnBeforeAnimating()
        {
            End = GetHostWidth();
            Easing = EasingMode.CubicIn;
        }

        protected override void Apply(double value, VisualElement view) => view.TranslationX = value;
    }
}
