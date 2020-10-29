using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public abstract class SlideTransition : Transition
    {
        private static Page GetHost() => Application.Current.MainPage;

        protected double GetHostWidth() => GetHost().Width;

        protected double GetHostHeight() => GetHost().Height;
    }
}
