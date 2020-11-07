using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.NavView.Transitions;
using Xamarin.Forms;

namespace X.NavView.Navigations
{
    public interface INavigator
    {
        Task Push(ContentView view, bool animated);

        Task Push(ContentView view) => Push(view, true);

        Task Pop(bool animated);

        Task Pop() => Pop(true);

        IReadOnlyList<ContentView> NavigationStack { get; }
    }

    internal class NavigatorImpl : INavigator
    {

        private static readonly List<ContentView> navigationStack = new List<ContentView>();

        private static IHostView Host;

        public NavigatorImpl(IHostView host) => Host = host;

        public NavigatorImpl()
        {
        }

        public IReadOnlyList<ContentView> NavigationStack => navigationStack;

        private bool CanPop(out ContentView view)
        {
            lock (navigationStack)
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

        private void AddToStack(ContentView view)
        {
            lock (navigationStack)
            {
                navigationStack.Add(view);
            }
        }

        public Task Pop(bool animated)
        {
            if (CanPop(out var view))
            {
                return SwitchView(view, false, animated);
            }

            return Task.CompletedTask;
        }

        public Task Push(ContentView view, bool animated) => SwitchView(view, true, animated);

        private async Task SwitchView(ContentView view, bool isPush, bool animated)
        {
            var tasks = new List<Task>();

            var viewController = view as IXViewController;

            if (isPush)
            {
                Host.SendPushing(view);

                viewController?.SendAppearing();

                if (CanPeek(out var peekView))
                {
                    tasks.Add(HideTransition(peekView, animated));

                    if (peekView is IXViewController pvc)
                    {
                        pvc.SendDisappearing();
                    }
                }

                Host.SendNavigateTo(peekView, view);

                AddToStack(view);
                await AddToHost(view);

                tasks.Add(EnterTransition(view, animated)
                        .ContinueWith(_ => ViewVisible(PreviousViewFrom(view), false)));
            }
            else
            {
                Host.SendPopping(view);

                viewController?.SendDisappearing();

                if (CanPeek(out var peekView))
                {
                    await ViewVisible(peekView, true);

                    tasks.Add(RevealTransition(peekView, animated));

                    if (peekView is IXViewController pvc)
                    {
                        pvc.SendAppearing();
                    }
                }

                Host.SendNavigateTo(view, peekView);

                tasks.Add(ExitTransition(view, animated)
                            .ContinueWith(_ => RemoveFromHost(view)));
            }

            await Task.WhenAll(tasks)
                .ContinueWith(_ =>
                {
                    if (isPush)
                    {
                        Host.SendPushed(view);
                    }
                    else
                    {
                        Host.SendPopped(view);
                    }
                });
        }

        private ContentView PreviousViewFrom(ContentView view)
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

        private async Task ExitTransition(ContentView view, bool animated)
        {
            if (view != null)
            {

                var t = Interaction.GetExit(view);

                await RunTransition(t, animated);
            }
        }

        private async Task RevealTransition(ContentView view, bool animated)
        {
            if (view != null)
            {
                var t = Interaction.GetReveal(view);

                await RunTransition(t, animated);
            }
        }

        private async Task HideTransition(ContentView view, bool animated)
        {
            if (view != null)
            {
                var t = Interaction.GetHide(view);

                await RunTransition(t, animated);
            }
        }

        private async Task EnterTransition(ContentView view, bool animated)
        {
            if (view != null)
            {
                var t = Interaction.GetEnter(view);

                await RunTransition(t, animated);
            }
        }

        private async Task ViewVisible(ContentView view, bool visible)
        {
            if (view != null)
            {
                await ThreadSafe(() => view.IsVisible = visible);
            }
        }

        private async Task RemoveFromHost(ContentView view)
        {
            if (view != null)
            {
                await ThreadSafe(() => Host.Remove(view));
            }
        }

        private async Task AddToHost(ContentView view)
        {
            if (view != null)
            {
                await ThreadSafe(() => Host.Add(view));
            }
        }

        private Task ThreadSafe(Action action) => Device.InvokeOnMainThreadAsync(action);

        private async Task RunTransition(ITransition transition, bool animated)
        {
            if (animated && transition != null)
            {
                await transition.Run();
            }
        }
    }
}