﻿using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public class SlideFromBottomEnter : SlideTransition
    {
        protected override void OnBeforeAnimating()
        {
            Start = GetHostHeight();
            End = 0;
            Easing = EasingMode.CubicOut;
        }

        protected override void Apply(double value, VisualElement view)
        {
            view.TranslationY = value;
        }
    }

    public class SlideToBottomExit : SlideTransition
    {
        protected override void OnBeforeAnimating()
        {
            End = GetHostHeight();
            Easing = EasingMode.CubicIn;
        }

        protected override void Apply(double value, VisualElement view)
        {
            view.TranslationY = value;
        }
    }

    
}