using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xam.Anim;
using Xamarin.Forms;

namespace X.NavView.Transitions
{

    public class Transition<TVisualElement> : ITransition where TVisualElement : VisualElement
    {
        internal TVisualElement View { get; set; }

        private string animationName;

        public BindableProperty TargetProperty { get; set; }

        public double? From { get; set; }
        public double? To { get; set; }

        public int Duration { get; set; } = 250;

        public EasingMode Easing { get; set; } = EasingMode.Linear;


        public Transition()
        {
            animationName = Guid.NewGuid().ToString();
        }

        public Transition(string name, BindableProperty targetProperty)
        {
            animationName = name;
            TargetProperty = targetProperty;
        }

        public Transition(BindableProperty targetProperty) : this(Guid.NewGuid().ToString(), targetProperty) { }

        public void Cancel()
        {
            if(View is IAnimatable view)
            {
                view.AbortAnimation(animationName);
            }
        }
        public Task Run()
        {
            if(View != null)
            {
                var propertyName = TargetProperty.PropertyName;

                var type = typeof(TVisualElement);
                var propertyInfo = type.GetProperty(propertyName);

                var from = From ?? (double)propertyInfo.GetValue(View);
                var to = To ?? 1.0;
                var duration = (uint)Duration;
                var easing = SwitchEasing(Easing);

                return RunAnimation(animationName, propertyName, from, to, duration, easing);
            }

            return Task.CompletedTask;
        }

        protected virtual Task RunAnimation(string animationName, string propertyName, double from, double to, uint duration, Easing easing)
        {
            return View.Animate(animationName, propertyName, from, to, duration, easing);
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

    public class Transitions : List<ITransition>, ITransition
    {
        internal VisualElement View { get; set; }

        public bool Sequential { get; set; } = false;

        public void Cancel() => this.ForEach(t => t.Cancel());
        public async Task Run()
        {
            if(Sequential)
            {
                foreach(var t in this)
                {
                    await t.Run();
                }
            }
            else
            {
                var tasks = new List<Task>();

                foreach(var t in this)
                {
                    tasks.Add(t.Run());
                }

                await Task.WhenAll(tasks);
            }
        }

        internal void SetView(VisualElement element)
        {
            foreach(var item in this)
            {
                if(item is Transition t)
                {
                    t.View = element;
                }
                else if(item is Transitions ts)
                {
                    ts.SetView(element);
                }
            }
        }

        internal void UnsetView()
        {
            foreach (var item in this)
            {
                if (item is Transition t)
                {
                    t.View = null;
                }
                else if (item is Transitions ts)
                {
                    ts.UnsetView();
                }
            }
        }
    }
}
