﻿using System;
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

        private static IHostViewController Host;

        public NavigationForViewImpl(HostView host) => Host = host;

        public NavigationForViewImpl()
        {
        }

        private bool CanPop(out ContentView view)
        {
            view = default;
            return navigationStack.Count > 1 && navigationStack.TryPop(out view);
        }

        private bool CanPopModal(out ContentView view)
        {
            return navigationModalStack.TryPop(out view);
        }

        private bool CanPeek(out ContentView view)
        {
            return navigationStack.TryPeek(out view);
        }

        private bool CanPeekModal(out ContentView view)
        {
            return navigationModalStack.TryPeek(out view);
        }

        private void AddToStack(ContentView view)
        {
            navigationStack.Push(view);
        }

        private void AddToModalStack(ContentView view)
        {
            navigationModalStack.Push(view);
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
                Host.SendPopping(view);
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
                          .ContinueWith(_ => ThreadSafeTask(() => Host.Remove(view)));

                if (controller != null)
                {
                    await controller.SendPopped();
                }

                Host.SendPopped(view);
            }
        }

        public async Task PushAsync(ContentView view, bool animated)
        {
            IDefaultViewController controller = view as IDefaultViewController;

            if (controller != null && !await controller.SendPushing())
            {
                return;
            }

            Host.SendPushing(view);
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

            await ThreadSafeTask(() => Host.Add(view));
            AddToStack(view);

            //then animate the pushed view.
            var enterTransition = Interaction.GetEnter(view);
            tasks.Add(RunTransition(enterTransition, view, animated));

            await Task.WhenAll(tasks);

            if (controller != null)
            {
                await controller.SendPushed();
            }

            Host.SendPushed(view);
        }

        public async Task PushModalAsync(ContentView view, bool animated)
        {
            IDefaultViewController controller = view as IDefaultViewController;

            if (controller != null && !await controller.SendPushing())
            {
                return;
            }

            await ThreadSafeTask(() => Host.ModalContainer.IsVisible = true);

            Host.SendPushingModal(view);
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
            var container = Host.Container;
            var hideTransition = ContainerInteraction.GetHide(view);

            tasks.Add(RunTransition(hideTransition, container, animated));

            //modal container
            var modalContainer = Host.ModalContainer;
            var revealTransition = ModalContainerInteraction.GetReveal(modalContainer);

            tasks.Add(RunTransition(revealTransition, modalContainer, animated));

            await ThreadSafeTask(() => Host.AddModal(view));
            AddToModalStack(view);

            var enterTransition = ModalInteraction.GetEnter(view);

            tasks.Add(RunTransition(enterTransition, view, animated));

            await Task.WhenAll(tasks);

            if (controller != null)
            {
                await controller.SendPushed();
            }
            Host.SendPushedModal(view);
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
                Host.SendPoppingModal(viewModal);
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
                    var modalContainer = Host.ModalContainer;
                    var hideTransition = ModalContainerInteraction.GetHide(modalContainer);

                    tasks.Add(RunTransition(hideTransition, modalContainer, animated));

                    //container
                    var container = Host.Container;
                    var revealTransition = ContainerInteraction.GetReveal(viewModal);

                    tasks.Add(RunTransition(revealTransition, container, animated));
                }

                var exitTransition = ModalInteraction.GetExit(viewModal);

                tasks.Add(RunTransition(exitTransition, viewModal, animated));

                await Task.WhenAll(tasks)
                    .ContinueWith(_ => ThreadSafeTask(() =>
                    {
                        Host.RemoveModal(viewModal);

                        if (navigationModalStack.Count == 0)
                        {
                            Host.ModalContainer.IsVisible = false;
                        }
                    }));

                if (controller != null)
                {
                    await controller.SendPopped();
                }
                Host.SendPoppedModal(viewModal);
            }
        }

        private Task ThreadSafeTask(Action action) => Device.InvokeOnMainThreadAsync(action);

        private async Task RunTransition(ITransition transition, VisualElement element, bool animated)
        {
            if (animated && transition != null)
            {
                await transition.Run(element);
            }
        }
    }
}