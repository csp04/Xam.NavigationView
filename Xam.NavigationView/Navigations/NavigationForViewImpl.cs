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
        private static readonly List<ContentView> navigationStack = new List<ContentView>();
        private static readonly List<ContentView> navigationModalStack = new List<ContentView>();

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
            lock (navigationModalStack)
            {
                view = default;

                if (navigationStack.Count > 1)
                {
                    view = navigationStack[^1];
                    navigationStack.RemoveAt(navigationStack.Count - 1);

                    return true;
                }

                return false;
            }
        }

        private bool CanPopModal(out ContentView view)
        {
            lock (navigationModalStack)
            {
                view = default;

                if (navigationModalStack.Count > 0)
                {
                    view = navigationModalStack[^1];
                    navigationModalStack.RemoveAt(navigationModalStack.Count - 1);

                    return true;
                }

                return false;
            }
        }

        private bool CanPeek(out ContentView view)
        {
            lock (navigationStack)
            {
                view = default;

                if (navigationStack.Count > 0)
                {
                    view = navigationStack[^1];
                    return true;
                }

                return false;
            }
        }

        private bool CanPeekModal(out ContentView view)
        {
            lock (navigationModalStack)
            {
                view = default;

                if (navigationModalStack.Count > 0)
                {
                    view = navigationModalStack[^1];
                    return true;
                }

                return false;
            }
        }

        private void AddToStack(ContentView view)
        {
            lock (navigationStack)
            {
                navigationStack.Add(view);
            }
        }

        private void AddToModalStack(ContentView view)
        {
            lock (navigationModalStack)
            {
                navigationModalStack.Add(view);
            }
        }

        private ContentView GetPreviousViewFrom(ContentView view)
        {
            lock (navigationStack)
            {
                var navList = navigationStack;

                var index = navList.IndexOf(view);

                if (index <= 0)
                {
                    return null;
                }

                return navList[index - 1];
            }
        }

        private ContentView GetPreviousViewFromModal(ContentView view)
        {
            lock (navigationModalStack)
            {
                var navList = navigationModalStack;

                var index = navList.IndexOf(view);

                if (index <= 0)
                {
                    return null;
                }

                return navList[index - 1];
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
                Host.SendPopping(view);
                _ = controller?.SendDisappearing();

                var tasks = new List<Task>();

                //get the current view
                //get the reveal animation for currentView
                if (CanPeek(out var currentView))
                {
                    currentView.IsVisible = true;

                    await ThreadSafeTask(() => Host.InsertBefore(currentView, view));

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

            await Task.WhenAll(tasks)
                .ContinueWith(async _ =>
                                    {
                                        var prevView = GetPreviousViewFrom(view);

                                        if (prevView != null)
                                        {
                                            await ThreadSafeTask(() => Host.Remove(prevView));
                                        }
                                    });

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

            ContentView currentView = default;

            if (CanPeekModal(out var currentModalView) && currentModalView is IDefaultViewController modalController)
            {
                _ = modalController.SendDisappearing();
            }
            else if (CanPeek(out currentView) && currentView is IDefaultViewController currentController)
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

            await Task.WhenAll(tasks)
                .ContinueWith(_ =>
                {

                    var prevView = GetPreviousViewFromModal(view);

                    if (prevView != null)
                    {
                        ThreadSafeTask(() => Host.RemoveModal(prevView));
                    }


                    if (currentView != null)
                    {
                        ThreadSafeTask(() => currentView.IsVisible = false);
                    }
                });

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
                    await ThreadSafeTask(() => Host.InsertBeforeModal(currentViewModal, viewModal));

                    if (currentViewModal is IDefaultViewController modalController)
                    {
                        _ = modalController.SendAppearing();
                    }
                }
                else
                {
                    if (CanPeek(out var currentView) && currentView is IDefaultViewController viewController)
                    {
                        currentView.IsVisible = true;
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