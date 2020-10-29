
using Xamarin.Forms;

namespace Xam.NavigationView
{
    internal interface IHostViewController
    {
        void Add(ContentView view);
        void AddModal(ContentView view);

        void Remove(ContentView view);

        void RemoveModal(ContentView view);

        void SendPushing(ContentView view);
        void SendPopping(ContentView view);
        void SendPushed(ContentView view);
        void SendPopped(ContentView view);

        void SendPushingModal(ContentView view);
        void SendPoppingModal(ContentView view);
        void SendPushedModal(ContentView view);
        void SendPoppedModal(ContentView view);


        VisualElement Container { get; }
        VisualElement ModalContainer { get; }
    }
}