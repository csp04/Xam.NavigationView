using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xam.NavigationView.Transitions;
using Xamarin.Forms;

namespace Xam.NavigationView
{
    internal class NavigationForViewImpl : INavigationForView
    {
        private static readonly Stack<ContentView> navigationStack = new Stack<ContentView>();
        private static readonly Stack<ContentView> navigationModalStack = new Stack<ContentView>();

        public IReadOnlyList<ContentView> NavigationStack
        {
            get
            {
                lock (navigationStack)
                {
                    return navigationStack.ToList();
                }
            }
        }

        public IReadOnlyList<ContentView> NavigationModalStack
        {
            get
            {
                lock (navigationModalStack)
                {
                    return navigationModalStack.ToList();
                }
            }
        }

        private readonly IHostViewController host;

        public NavigationForViewImpl(HostView host)
        {
            this.host = host;
        }

        public NavigationForViewImpl()
        {

        }

        private bool CanPop(out ContentView view)
        {
            view = default;

            lock (navigationStack)
            {
                return navigationStack.Count > 1 && navigationStack.TryPop(out view);
            }
        }

        private bool CanPopModal(out ContentView view)
        {
            view = default;

            lock (navigationModalStack)
            {
                return navigationModalStack.TryPop(out view);
            }
        }

        private bool CanPeek(out ContentView view)
        {
            view = default;

            lock (navigationStack)
            {
                return navigationStack.TryPeek(out view);
            }
        }

        private bool CanPeekModal(out ContentView view)
        {
            view = default;

            lock (navigationModalStack)
            {
                return navigationModalStack.TryPeek(out view);
            }
        }

        private void AddToStack(ContentView view)
        {
            lock (navigationStack)
            {
                navigationStack.Push(view);
            }
        }

        private void AddToModalStack(ContentView view)
        {
            lock (navigationModalStack)
            {
                navigationModalStack.Push(view);
            }
        }


        public async Task PopAsync(bool animated)
        {
            IDefaultViewController controller = default;

            if (CanPeek(out var _view) && _view is IDefaultViewController _controller)
            {
                controller = _controller;

                if (!await controller.SendPopping())
                {
                    return;
                }

            }

            if (CanPop(out var view))
            {
                host.SendPopping(view);
                _ = controller?.SendDisappearing();

                var tasks = new List<Task>();

                //get the current view
                //get the reveal animation for currentView
                if (CanPeek(out var currentView))
                {
                    var revealTransition = Interaction.GetReveal(currentView);
                    tasks.Add(RunTransition(revealTransition, currentView, animated));

                    if (currentView is IDefaultViewController currentController)
                    {
                        _ = currentController.SendAppearing();
                    }
                }

                //get the exit animation
                var exitTransition = Interaction.GetExit(view);
                tasks.Add(RunTransition(exitTransition, view, animated));

                await Task.WhenAll(tasks)
                          .ContinueWith(_ => ThreadSafeTask(() => host.Remove(view)));

                if (controller != null)
                {
                    await controller.SendPopped();
                }

                host.SendPopped(view);
            }
        }

        public async Task PushAsync(ContentView view, bool animated)
        {
            IDefaultViewController controller = view as IDefaultViewController;

            if (controller != null && !await controller.SendPushing())
            {
                return;
            }

            host.SendPushing(view);
            _ = controller?.SendAppearing();

            var tasks = new List<Task>();

            //get the current view
            if (CanPeek(out var currentView))
            {
                //hide animation for the currentView.
                var hideTransition = Interaction.GetHide(currentView);
                tasks.Add(RunTransition(hideTransition, currentView, animated));

                if (currentView is IDefaultViewController currentController)
                {
                    _ = currentController.SendDisappearing();
                }
            }

            await ThreadSafeTask(() => host.Add(view));
            AddToStack(view);

            //then animate the pushed view.
            var enterTransition = Interaction.GetEnter(view);
            tasks.Add(RunTransition(enterTransition, view, animated));

            await Task.WhenAll(tasks);

            if (controller != null)
            {
                await controller.SendPushed();
            }

            host.SendPushed(view);
        }

        public async Task PushModalAsync(ContentView view, bool animated)
        {
            IDefaultViewController controller = view as IDefaultViewController;

            if (controller != null && !await controller.SendPushing())
            {
                return;
            }

            await ThreadSafeTask(() => host.ModalContainer.IsVisible = true);

            host.SendPushingModal(view);
            _ = controller?.SendAppearing();

            if (CanPeekModal(out var currentModalView) && currentModalView is IDefaultViewController modalController)
            {
                _ = modalController.SendDisappearing();
            }
            else if (CanPeek(out var currentView) && currentView is IDefaultViewController currentController)
            {
                _ = currentController.SendDisappearing();
            }

            var tasks = new List<Task>();

            //container
            var container = host.Container;
            var hideTransition = ContainerInteraction.GetHide(view);

            tasks.Add(RunTransition(hideTransition, container, animated));

            //modal container
            var modalContainer = host.ModalContainer;
            var revealTransition = ModalContainerInteraction.GetReveal(modalContainer);

            tasks.Add(RunTransition(revealTransition, modalContainer, animated));

            await ThreadSafeTask(() => host.AddModal(view));
            AddToModalStack(view);

            var enterTransition = ModalInteraction.GetEnter(view);

            tasks.Add(RunTransition(enterTransition, view, animated));

            await Task.WhenAll(tasks);

            if (controller != null)
            {
                await controller.SendPushed();
            }
            host.SendPushedModal(view);
        }

        public async Task PopModalAsync(bool animated)
        {
            IDefaultViewController controller = default;

            if (CanPeekModal(out var _view) && _view is IDefaultViewController _controller)
            {
                controller = _controller;

                if (!await controller.SendPopping())
                {
                    return;
                }
            }

            if (CanPopModal(out var viewModal))
            {
                host.SendPoppingModal(viewModal);
                _ = controller?.SendDisappearing();

                var tasks = new List<Task>();

                if (CanPeekModal(out var currentViewModal))
                {
                    if (currentViewModal is IDefaultViewController modalController)
                    {
                        _ = modalController.SendAppearing();
                    }
                }
                else
                {
                    if (CanPeek(out var currentView) && currentView is IDefaultViewController viewController)
                    {
                        _ = viewController.SendAppearing();
                    }

                    //modal container
                    var modalContainer = host.ModalContainer;
                    var hideTransition = ModalContainerInteraction.GetHide(modalContainer);

                    tasks.Add(RunTransition(hideTransition, modalContainer, animated));

                    //container
                    var container = host.Container;
                    var revealTransition = ContainerInteraction.GetReveal(viewModal);

                    tasks.Add(RunTransition(revealTransition, container, animated));
                }

                var exitTransition = ModalInteraction.GetExit(viewModal);

                tasks.Add(RunTransition(exitTransition, viewModal, animated));

                await Task.WhenAll(tasks)
                    .ContinueWith(_ => ThreadSafeTask(() =>
                    {
                        host.RemoveModal(viewModal);

                        if (navigationModalStack.Count == 0)
                        {
                            host.ModalContainer.IsVisible = false;
                        }
                    }));

                if (controller != null)
                {
                    await controller.SendPopped();
                }
                host.SendPoppedModal(viewModal);
            }
        }

        private Task ThreadSafeTask(Action action)
        {
            return Device.InvokeOnMainThreadAsync(action);
        }

        private async Task RunTransition(ITransition transition, VisualElement element, bool animated)
        {
            if (animated && transition != null)
            {
                await transition.Run(element);
            }
        }
    }
}