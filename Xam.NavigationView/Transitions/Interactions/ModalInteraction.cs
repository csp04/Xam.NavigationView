using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{

    public static class ModalInteraction
    {
        #region Enter Dependency Property
        public static void SetEnter(this BindableObject obj, ITransition value) => obj.SetValue(EnterProperty, value);

        public static ITransition GetEnter(this BindableObject obj) => (ITransition)obj.GetValue(EnterProperty);

        public static readonly BindableProperty EnterProperty = BindableProperty.Create("Enter", typeof(ITransition), typeof(ModalInteraction), default(ITransition));
        #endregion
        #region Exit Dependency Property
        public static void SetExit(this BindableObject obj, ITransition value) => obj.SetValue(ExitProperty, value);

        public static ITransition GetExit(this BindableObject obj) => (ITransition)obj.GetValue(ExitProperty);

        public static readonly BindableProperty ExitProperty = BindableProperty.Create("Exit", typeof(ITransition), typeof(ModalInteraction), default(ITransition));
        #endregion
    }

}
