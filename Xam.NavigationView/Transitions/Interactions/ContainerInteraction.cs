using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public static class ContainerInteraction
    {
        #region Hide Dependency Property
        public static void SetHide(this BindableObject obj, ITransition value) => obj.SetValue(HideProperty, value);

        public static ITransition GetHide(this BindableObject obj) => (ITransition)obj.GetValue(HideProperty);

        public static readonly BindableProperty HideProperty = BindableProperty.Create("Hide", typeof(ITransition), typeof(ContainerInteraction), default(ITransition));
        #endregion
        #region Reveal Dependency Property
        public static void SetReveal(this BindableObject obj, ITransition value) => obj.SetValue(RevealProperty, value);

        public static ITransition GetReveal(this BindableObject obj) => (ITransition)obj.GetValue(RevealProperty);

        public static readonly BindableProperty RevealProperty = BindableProperty.Create("Reveal", typeof(ITransition), typeof(ContainerInteraction), default(ITransition));
        #endregion
    }

}
