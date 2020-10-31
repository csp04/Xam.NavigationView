
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
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

        private TaskQueue tq = new TaskQueue();

        public HostView(ContentView view)
        {
            InitializeComponent();

            Navigation = new NavigationForViewImpl(this);

            Navigation.PushAsync(view);
        }

        void IHostViewController.Add(ContentView view)
        {
            tq.Enqueue(() => container.Children.Add(view));
        }

        void IHostViewController.AddModal(ContentView view)
        {
            tq.Enqueue(() => modalContainer.Add(view));
        }

        void IHostViewController.Remove(ContentView view)
        {

            tq.Enqueue(() => container.Children.Remove(view));

        }

        void IHostViewController.RemoveModal(ContentView view)
        {


            tq.Enqueue(() => modalContainer.Remove(view));

        }

        void IHostViewController.InsertBefore(ContentView viewToInsert, ContentView beforeThisView)
        {

            var index = container.Children.IndexOf(beforeThisView);

            if (index < 0)
            {
                return;
            }

            tq.Enqueue(() => container.Children.Insert(index, viewToInsert));

        }

        void IHostViewController.InsertBeforeModal(ContentView viewToInsert, ContentView beforeThisView)
        {

            var index = modalContainer.IndexOf(beforeThisView);

            if (index < 0)
            {
                return;
            }

            tq.Enqueue(() => modalContainer.Insert(index, viewToInsert));

        }


        protected override bool OnBackButtonPressed()
        {
            if (Navigation.NavigationModalStack.Count > 0)
            {
                _ = Navigation.PopModalAsync()
                    .ContinueWith(_ =>
                    {
                        if (_.IsFaulted)
                            Debug.WriteLine(_.Exception);
                    });

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
        void IHostViewController.SendPushing(ContentView view) => Pushing?.Invoke(this, view);

        void IHostViewController.SendPopping(ContentView view) => Popping?.Invoke(this, view);

        void IHostViewController.SendPushed(ContentView view) => Pushed?.Invoke(this, view);

        void IHostViewController.SendPopped(ContentView view) => Popped?.Invoke(this, view);

        void IHostViewController.SendPushingModal(ContentView view) => PushingModal?.Invoke(this, view);

        void IHostViewController.SendPoppingModal(ContentView view) => PoppingModal?.Invoke(this, view);

        void IHostViewController.SendPushedModal(ContentView view) => PushedModal?.Invoke(this, view);

        void IHostViewController.SendPoppedModal(ContentView view) => PoppedModal?.Invoke(this, view);

        private class TaskQueue
        {
            object gate = new object();
            private Queue<Action> tasks = new Queue<Action>();
            private bool isBusy = false;

            public void Enqueue(Action task)
            {
                lock (gate)
                {
                    if(isBusy)
                    {
                        tasks.Enqueue(task);
                    }
                    else
                    {
                        isBusy = true;
                        task();
                        isBusy = false;

                        while(tasks.Count > 0)
                            tasks.Dequeue().Invoke();
                    }
                    
                }
            }

        }
    }
}