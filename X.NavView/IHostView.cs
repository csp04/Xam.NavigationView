using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace X.NavView
{
    public interface IHostView
    {
        void Remove(ContentView view);
        void Add(ContentView view);
        void SendPushing(ContentView view);
        void SendPopping(ContentView view);
        void SendPushed(ContentView view);
        void SendPopped(ContentView view);
        void SendNavigateTo(ContentView fromView, ContentView toView);
    }
}