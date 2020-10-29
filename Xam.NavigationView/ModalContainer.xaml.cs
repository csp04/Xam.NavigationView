
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xam.NavigationView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModalContainer : ContentView
    {
        internal ModalContainer()
        {
            InitializeComponent();
        }

        public void Add(View view)
        {
            container.Children.Add(view);
        }

        public void Remove(View view)
        {
            container.Children.Remove(view);
        }

        public new double Opacity
        {
            get => bgContainer.Opacity;
            set => bgContainer.Opacity = value;
        }
    }
}