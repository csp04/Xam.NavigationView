using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xam.NavigationView
{
    public abstract class DefaultView : ContentView, IDefaultViewController
    {
        public new INavigationForView Navigation { get; } = new NavigationForViewImpl();


        public DefaultView() => BackgroundColor = Color.White;

        protected virtual Task<bool> OnPushing() => Task.FromResult(true);

        protected virtual Task<bool> OnPopping() => Task.FromResult(true);

        protected virtual Task OnPushed() => Task.CompletedTask;

        protected virtual Task OnPopped() => Task.CompletedTask;

        protected virtual Task OnAppearing() => Task.CompletedTask;

        protected virtual Task OnDisappearing() => Task.CompletedTask;

        Task<bool> IDefaultViewController.SendPushing() => OnPushing();

        Task IDefaultViewController.SendPushed() => OnPushed();

        Task<bool> IDefaultViewController.SendPopping() => OnPopping();

        Task IDefaultViewController.SendPopped() => OnPopped();

        Task IDefaultViewController.SendAppearing() => OnAppearing();

        Task IDefaultViewController.SendDisappearing() => OnDisappearing();
    }


    public interface IDefaultViewController
    {
        Task<bool> SendPushing();
        Task SendPushed();

        Task<bool> SendPopping();
        Task SendPopped();

        Task SendAppearing();
        Task SendDisappearing();
    }
}
