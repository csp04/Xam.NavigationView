
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xam.NavigationView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModalContainer : ContentView
    {
        internal ModalContainer() => InitializeComponent();

        public void Add(View view) => container.Children.Add(view);

        public void Remove(View view) => container.Children.Remove(view);

        public void Insert(int index, View view) => container.Children.Insert(index, view);

        public int IndexOf(View view) => container.Children.IndexOf(view);

        public new double Opacity
        {
            get => bgContainer.Opacity;
            set => bgContainer.Opacity = value;
        }
    }
}