using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Xam.NavigationView
{
    public interface INavigationForView
    {
        Task PushAsync(ContentView view, bool animated);
        Task PushAsync(ContentView view)
        {
            return PushAsync(view, true);
        }

        Task PopAsync(bool animated);
        Task PopAsync()
        {
            return PopAsync(true);
        }

        Task PushModalAsync(ContentView view, bool animated);
        Task PopModalAsync(bool animated);

        Task PushModalAsync(ContentView view)
        {
            return PushModalAsync(view, true);
        }

        Task PopModalAsync()
        {
            return PopModalAsync(true);
        }

        public IReadOnlyList<ContentView> NavigationStack { get; }

        public IReadOnlyList<ContentView> NavigationModalStack { get; }

    }
}