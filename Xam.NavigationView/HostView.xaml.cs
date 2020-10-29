
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xam.NavigationView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HostView : ContentPage, IHostViewController
    {
        public new INavigationForView Navigation { get; }
        VisualElement IHostViewController.Container => container;
        VisualElement IHostViewController.ModalContainer => modalContainer;

        public HostView(ContentView view)
        {
            InitializeComponent();

            Navigation = new NavigationForViewImpl(this);

            Navigation.PushAsync(view);
        }

        void IHostViewController.Add(ContentView view)
        {
            container.Children.Add(view);
        }

        void IHostViewController.AddModal(ContentView view)
        {
            modalContainer.Add(view);
        }

        void IHostViewController.Remove(ContentView view)
        {
            container.Children.Remove(view);
        }

        void IHostViewController.RemoveModal(ContentView view)
        {
            modalContainer.Remove(view);
        }

        protected override bool OnBackButtonPressed()
        {
            if (Navigation.NavigationModalStack.Count > 0)
            {
                _ = Navigation.PopModalAsync();

                return true;
            }

            if (Navigation.NavigationStack.Count > 1)
            {
                _ = Navigation.PopAsync();

                return true;
            }

            return base.OnBackButtonPressed();
        }

        public event EventHandler<ContentView> Pushing, Popping, Pushed, Popped, PushingModal, PoppingModal, PushedModal, PoppedModal;
        void IHostViewController.SendPushing(ContentView view)
        {
            Pushing?.Invoke(this, view);
        }

        void IHostViewController.SendPopping(ContentView view)
        {
            Popping?.Invoke(this, view);
        }

        void IHostViewController.SendPushed(ContentView view)
        {
            Pushed?.Invoke(this, view);
        }

        void IHostViewController.SendPopped(ContentView view)
        {
            Popped?.Invoke(this, view);
        }

        void IHostViewController.SendPushingModal(ContentView view)
        {
            PushingModal?.Invoke(this, view);
        }

        void IHostViewController.SendPoppingModal(ContentView view)
        {
            PoppingModal?.Invoke(this, view);
        }

        void IHostViewController.SendPushedModal(ContentView view)
        {
            PushedModal?.Invoke(this, view);
        }

        void IHostViewController.SendPoppedModal(ContentView view)
        {
            PoppedModal?.Invoke(this, view);
        }
    }
}