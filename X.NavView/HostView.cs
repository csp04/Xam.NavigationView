using X.NavView.Navigations;
using Xamarin.Forms;

namespace X.NavView
{
    public class HostView : ContentPage, IHostView
    {
        protected Grid Container => Content as Grid;

        public INavigator Navigator { get; }

        public HostView(ContentView root)
        {
            Navigator = new NavigatorImpl(this);

            Content = new Grid();

            Navigator.Push(root);
        }

        protected virtual void OnPopping(ContentView view) { }
        protected virtual void OnPushing(ContentView view) { }
        protected virtual void OnPopped(ContentView view) { }
        protected virtual void OnPushed(ContentView view) { }
        protected virtual void OnNavigateTo(ContentView fromView, ContentView toView) { }

        void IHostView.Add(ContentView view) => Container.Children.Add(view);
        void IHostView.Remove(ContentView view) => Container.Children.Remove(view);
        void IHostView.SendPopped(ContentView view) => OnPopped(view);
        void IHostView.SendPopping(ContentView view) => OnPopping(view);
        void IHostView.SendPushed(ContentView view) => OnPushed(view);
        void IHostView.SendPushing(ContentView view) => OnPushing(view);
        void IHostView.SendNavigateTo(ContentView fromView, ContentView toView) => OnNavigateTo(fromView, toView);

        protected override bool OnBackButtonPressed()
        {
            if (Navigator.NavigationStack.Count > 1)
            {
                Navigator.Pop();

                return true;
            }

            return base.OnBackButtonPressed();
        }

    }
}