using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xam.NavigationView
{
    public abstract class DefaultView : ContentView, IDefaultViewController
    {
        public new INavigationForView Navigation { get; } = new NavigationForViewImpl();


        public DefaultView()
        {
            BackgroundColor = Color.White;
        }

        protected virtual Task<bool> OnPushing()
        {
            return Task.FromResult(true);
        }

        protected virtual Task<bool> OnPopping()
        {
            return Task.FromResult(true);
        }

        protected virtual Task OnPushed()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnPopped()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnAppearing()
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnDisappearing()
        {
            return Task.CompletedTask;
        }

        Task<bool> IDefaultViewController.SendPushing()
        {
            return OnPushing();
        }

        Task IDefaultViewController.SendPushed()
        {
            return OnPushed();
        }

        Task<bool> IDefaultViewController.SendPopping()
        {
            return OnPopping();
        }

        Task IDefaultViewController.SendPopped()
        {
            return OnPopped();
        }

        Task IDefaultViewController.SendAppearing()
        {
            return OnAppearing();
        }

        Task IDefaultViewController.SendDisappearing()
        {
            return OnDisappearing();
        }
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
