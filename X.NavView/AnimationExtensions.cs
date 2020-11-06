using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.NavView.Helpers;
using Xamarin.Forms;

namespace X.NavView
{
    public static class AnimationExtensions
    {
        public static Task Animate<TVisualElement>(
            this TVisualElement element, 
            string name,
            Action<double, VisualElement> apply,
            double from, double to, uint duration = 250,
            Easing easing = null
                ) where TVisualElement : VisualElement
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            element.Animate(name, d => apply(d, element), from, to, 
                length: duration, easing: easing, finished: (d, f) => tcs.SetResult(!f));
            
            return tcs.Task;
        }

        public static Task Animate<TVisualElement>(
            this TVisualElement element,
            string name,
            string propertyName,
            double from, double to, uint duration = 250,
            Easing easing = null
                ) where TVisualElement : VisualElement
        {
            var propertyInfo = typeof(TVisualElement).GetProperty(propertyName);

            return Animate(element, name, (d,x) => propertyInfo.SetValue(x, d), from, to, duration, easing);
        }

        public static Task Animate<TVisualElement>(
            this TVisualElement element,
            string name,
            Expression<Func<TVisualElement, double>> propertyExpression,
            double from, double to, uint duration = 250,
            Easing easing = null
                ) where TVisualElement : VisualElement
        {
            return Animate(element, name, propertyExpression.GetPropertyName(), from, to, duration, easing);
        }

        public static Task AnimateTo<TVisualElement>(
            this TVisualElement element,
            string name,
            string propertyName,
            double to, uint duration = 250,
            Easing easing = null
                ) where TVisualElement : VisualElement
        {
            var propertyInfo = typeof(TVisualElement).GetProperty(propertyName);
            var from = (double)propertyInfo.GetValue(element);

            return Animate(element, name, propertyName, from, to, duration, easing);
        }

        public static Task AnimateTo<TVisualElement>(
            this TVisualElement element,
            string name,
            Expression<Func<TVisualElement, double>> propertyExpression, 
            double to, uint duration = 250,
            Easing easing = null
                ) where TVisualElement : VisualElement
        {
            return AnimateTo(element, name, propertyExpression.GetPropertyName(), to, duration, easing);
        }

        public static Task AnimatePercent<TVisualElement>(
            this TVisualElement element,
            string name,
            Action<double, VisualElement> apply,
            double fromPercentage, double toPercentage,
            double value,
            uint duration = 250,
            Easing easing = null
            ) where TVisualElement : VisualElement
        {

            var from = value * fromPercentage;
            var to = value * toPercentage;


            return Animate(element, name, apply, from, to, duration, easing);
        }

        public static Task AnimatePercent<TVisualElement>(
            this TVisualElement element,
            string name,
            string propertyName,
            double fromPercentage, double toPercentage,
            double value,
            uint duration = 250,
            Easing easing = null
            ) where TVisualElement : VisualElement
        {

            var from = value * fromPercentage;
            var to = value * toPercentage;

            return Animate(element, name, propertyName, from, to, duration, easing);
        }

        public static Task AnimatePercent<TVisualElement>(
            this TVisualElement element,
            string name,
            Expression<Func<TVisualElement, double>> propertyExpression,
            double fromPercentage, double toPercentage,
            double value,
            uint duration = 250,
            Easing easing = null
            ) where TVisualElement : VisualElement
        {

            var from = value * fromPercentage;
            var to = value * toPercentage;

            return Animate(element, name, propertyExpression.GetPropertyName(), from, to, duration, easing);
        }

        public static Task AnimatePercentTo<TVisualElement>(
            this TVisualElement element,
            string name,
            string propertyName,
            double toPercentage, double value, uint duration = 250,
            Easing easing = null
            ) where TVisualElement : VisualElement
        {

            var to = value * toPercentage;


            return AnimateTo(element, name, propertyName, to, duration, easing);
        }

        public static Task AnimatePercentTo<TVisualElement>(
            this TVisualElement element,
            string name,
            Expression<Func<TVisualElement, double>> propertyExpression,
            double toPercentage, double value, uint duration = 250,
            Easing easing = null
            ) where TVisualElement : VisualElement
        {

            var to = value * toPercentage;


            return AnimateTo(element, name, propertyExpression, to, duration, easing);
        }
    }

    
}
