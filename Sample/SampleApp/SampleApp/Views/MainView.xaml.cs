using System.Diagnostics;
using Xam.NavigationView;
using Xamarin.Forms.Xaml;

namespace SampleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : DefaultView
    {
        public MainView() => InitializeComponent();

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new SlideFromBottomView(), false);
            Navigation.PushAsync(new SlideFromTopView(), false);
            Navigation.PushAsync(new SlideFromLeftView(), false);
            Navigation.PushAsync(new SlideFromRightView(), false);
        }

        private void Button_Clicked_1(object sender, System.EventArgs e)
        {
            Navigation.PushModalAsync(new SlideFromBottomView());
            Navigation.PushModalAsync(new SlideFromTopView());
            Navigation.PushModalAsync(new SlideFromLeftView());
            Navigation.PushModalAsync(new SlideFromRightView());
        }
    }
}