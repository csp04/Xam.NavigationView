using System;
using System.Threading.Tasks;
using Xam.Anim;
using Xamarin.Forms;

namespace X.NavView.Transitions
{

    public class Transition<TVisualElement> : ITransition where TVisualElement : VisualElement
    {
        internal TVisualElement View { get; set; }

        private readonly string animationName;

        public BindableProperty TargetProperty { get; set; }

        public double? From { get; set; }
        public double? To { get; set; }

        public int Duration { get; set; } = 250;

        public EasingMode Easing { get; set; } = EasingMode.Linear;


        public Transition() => animationName = Guid.NewGuid().ToString();

        public Transition(string name, BindableProperty targetProperty)
        {
            animationName = name;
            TargetProperty = targetProperty;
        }

        public Transition(BindableProperty targetProperty) : this(Guid.NewGuid().ToString(), targetProperty) { }

        public void Cancel() => View.AbortAnimation(animationName);
        public Task Run()
        {
            if (View != null)
            {
                var propertyName = TargetProperty.PropertyName;

                var type = typeof(TVisualElement);
                var propertyInfo = type.GetProperty(propertyName);

                var from = From ?? GetPropertyValue((double)propertyInfo.GetValue(View));
                var to = To ?? 1.0;
                var duration = (uint)Duration;
                var easing = SwitchEasing(Easing);

                return RunAnimation(animationName, propertyName, from, to, duration, easing);
            }

            return Task.CompletedTask;
        }

        protected virtual double GetPropertyValue(double propertyValue) => propertyValue;

        protected virtual Task RunAnimation(string animationName, string propertyName, double from, double to, uint duration, Easing easing) => View.Animate(animationName, propertyName, from, to, duration, easing);

        private Easing SwitchEasing(EasingMode easing) => easing switch
        {
            EasingMode.Linear => Xamarin.Forms.Easing.Linear,
            EasingMode.SinOut => Xamarin.Forms.Easing.SinOut,
            EasingMode.SinIn => Xamarin.Forms.Easing.SinIn,
            EasingMode.SinInOut => Xamarin.Forms.Easing.SinInOut,
            EasingMode.CubicIn => Xamarin.Forms.Easing.CubicIn,
            EasingMode.CubicOut => Xamarin.Forms.Easing.CubicOut,
            EasingMode.CubicInOut => Xamarin.Forms.Easing.CubicInOut,
            EasingMode.BounceOut => Xamarin.Forms.Easing.BounceOut,
            EasingMode.BounceIn => Xamarin.Forms.Easing.BounceIn,
            EasingMode.SpringIn => Xamarin.Forms.Easing.SpringIn,
            EasingMode.SpringOut => Xamarin.Forms.Easing.SpringOut,
            _ => Xamarin.Forms.Easing.Linear
        };
    }

    public class Transition : Transition<VisualElement>
    {
        public Transition()
        {
        }

        public Transition(BindableProperty targetProperty) : base(targetProperty)
        {
        }

        public Transition(string name, BindableProperty targetProperty) : base(name, targetProperty)
        {
        }
    }
}
