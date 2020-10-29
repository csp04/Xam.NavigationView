using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Xam.NavigationView
{
    public interface INavigationForView
    {
        Task PushAsync(ContentView view, bool animated);
        Task PushAsync(ContentView view) => PushAsync(view, true);

        Task PopAsync(bool animated);
        Task PopAsync() => PopAsync(true);

        Task PushModalAsync(ContentView view, bool animated);
        Task PopModalAsync(bool animated);

        Task PushModalAsync(ContentView view) => PushModalAsync(view, true);

        Task PopModalAsync() => PopModalAsync(true);

        public IReadOnlyList<ContentView> NavigationStack { get; }

        public IReadOnlyList<ContentView> NavigationModalStack { get; }

    }
}