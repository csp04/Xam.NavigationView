using Xamarin.Forms;

namespace Xam.NavigationView.Transitions
{
    public abstract class SlideTransition : Transition
    {
        private static Page GetHost()
        {
            return Application.Current.MainPage;
        }

        protected double GetHostWidth()
        {
            return GetHost().Width;
        }

        protected double GetHostHeight()
        {
            return GetHost().Height;
        }
    }
}
