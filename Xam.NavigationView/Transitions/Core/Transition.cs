using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public abstract class Transition<TView> : ITransition where TView : VisualElement
    {
        private readonly string name;
        public Transition() => name = Guid.NewGuid().ToString();

        public double Start { get; set; } = 0;
        public double End { get; set; } = 1;

        public EasingMode Easing { get; set; } = EasingMode.Linear;

        public int Duration { get; set; } = 250;

        protected abstract void Apply(double value, TView view);
        protected virtual void OnBeforeAnimating() { }

        public Task Run(VisualElement element)
        {
            OnBeforeAnimating();

            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

            var length = Convert.ToUInt32(Duration);
            var easing = SwitchEasing(Easing);

            element.Animate(name, d => Apply(d, (TView)element), Start, End, length: length, easing: easing, finished: (d, f) => tcs.SetResult(true));

            return tcs.Task;
        }

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

    public abstract class Transition : Transition<VisualElement>
    {
    }
}
