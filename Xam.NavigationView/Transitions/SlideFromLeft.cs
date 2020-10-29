using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public class SlideFromLeftEnter : SlideTransition
    {
        protected override void OnBeforeAnimating()
        {
            Start = -GetHostWidth();
            End = 0;
            Easing = EasingMode.CubicOut;
        }

        protected override void Apply(double value, VisualElement view)
        {
            view.TranslationX = value;
        }
    }

    public class SlideToLeftExit : SlideTransition
    {
        protected override void OnBeforeAnimating()
        {
            End = -GetHostWidth();
            Easing = EasingMode.CubicIn;
        }

        protected override void Apply(double value, VisualElement view)
        {
            view.TranslationX = value;
        }
    }
}
