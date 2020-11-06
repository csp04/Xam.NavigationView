using System.Diagnostics;
using X.NavView;
using Xamarin.Forms.Xaml;

namespace SampleApp2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View1 : XContentView
    {
        public View1() => InitializeComponent();
        protected override void OnAppearing() => Debug.WriteLine($"{GetType()} Appearing");

        protected override void OnDisappearing() => Debug.WriteLine($"{GetType()} Disappearing");
    }
}