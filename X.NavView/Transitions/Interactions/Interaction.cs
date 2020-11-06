using Xamarin.Forms;

namespace X.NavView.Transitions
{
    public static class Interaction
    {
        #region Enter Dependency Property
        public static void SetEnter(this BindableObject obj, ITransition value) => obj.SetValue(EnterProperty, value);

        public static ITransition GetEnter(this BindableObject obj) => (ITransition)obj.GetValue(EnterProperty);

        public static readonly BindableProperty EnterProperty =
            BindableProperty.Create("Enter", typeof(ITransition),
                typeof(Interaction), default(ITransition), propertyChanged: OnTransitionSet);

        #endregion Enter Dependency Property

        #region Exit Dependency Property
        public static void SetExit(this BindableObject obj, ITransition value) => obj.SetValue(ExitProperty, value);

        public static ITransition GetExit(this BindableObject obj) => (ITransition)obj.GetValue(ExitProperty);

        public static readonly BindableProperty ExitProperty =
            BindableProperty.Create("Exit", typeof(ITransition),
                typeof(Interaction), default(ITransition), propertyChanged: OnTransitionSet);
        #endregion

        #region Hide Dependency Property
        public static void SetHide(this BindableObject obj, ITransition value) => obj.SetValue(HideProperty, value);

        public static ITransition GetHide(this BindableObject obj) => (ITransition)obj.GetValue(HideProperty);

        public static readonly BindableProperty HideProperty =
            BindableProperty.Create("Hide", typeof(ITransition),
                typeof(Interaction), default(ITransition), propertyChanged: OnTransitionSet);
        #endregion

        #region Reveal Dependency Property
        public static void SetReveal(this BindableObject obj, ITransition value) => obj.SetValue(RevealProperty, value);

        public static ITransition GetReveal(this BindableObject obj) => (ITransition)obj.GetValue(RevealProperty);

        public static readonly BindableProperty RevealProperty =
            BindableProperty.Create("Reveal", typeof(ITransition),
                typeof(Interaction), default(ITransition), propertyChanged: OnTransitionSet);
        #endregion


        private static void OnTransitionSet(BindableObject bindable, object oldValue, object newValue)
        {
            if (oldValue is Transition ot)
            {
                ot.View = null;
            }
            else if (oldValue is Transitions ots)
            {
                ots.UnsetView();
            }

            if (newValue is Transition nt)
            {
                nt.View = (VisualElement)bindable;
            }
            else if (newValue is Transitions nts)
            {
                nts.SetView((VisualElement)bindable);
            }
        }
    }
}
