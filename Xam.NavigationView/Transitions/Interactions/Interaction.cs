using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public static class Interaction
    {
        #region Enter Dependency Property
        public static void SetEnter(this BindableObject obj, ITransition value)
        {
            obj.SetValue(EnterProperty, value);
        }

        public static ITransition GetEnter(this BindableObject obj)
        {
            return (ITransition)obj.GetValue(EnterProperty);
        }

        public static readonly BindableProperty EnterProperty = BindableProperty.Create("Enter", typeof(ITransition), typeof(Interaction), default(ITransition));
        #endregion
        #region Exit Dependency Property
        public static void SetExit(this BindableObject obj, ITransition value)
        {
            obj.SetValue(ExitProperty, value);
        }

        public static ITransition GetExit(this BindableObject obj)
        {
            return (ITransition)obj.GetValue(ExitProperty);
        }

        public static readonly BindableProperty ExitProperty = BindableProperty.Create("Exit", typeof(ITransition), typeof(Interaction), default(ITransition));
        #endregion
        #region Hide Dependency Property
        public static void SetHide(this BindableObject obj, ITransition value)
        {
            obj.SetValue(HideProperty, value);
        }

        public static ITransition GetHide(this BindableObject obj)
        {
            return (ITransition)obj.GetValue(HideProperty);
        }

        public static readonly BindableProperty HideProperty = BindableProperty.Create("Hide", typeof(ITransition), typeof(Interaction), default(ITransition));
        #endregion
        #region Reveal Dependency Property
        public static void SetReveal(this BindableObject obj, ITransition value)
        {
            obj.SetValue(RevealProperty, value);
        }

        public static ITransition GetReveal(this BindableObject obj)
        {
            return (ITransition)obj.GetValue(RevealProperty);
        }

        public static readonly BindableProperty RevealProperty = BindableProperty.Create("Reveal", typeof(ITransition), typeof(Interaction), default(ITransition));
        #endregion

    }

}
