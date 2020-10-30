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
            Navigation.PushAsync(new SlideFromBottomView())
                .ContinueWith(t =>
                {
                    if(t.IsFaulted)
                    {
                        Debug.WriteLine(t.Exception);
                    }
                });
            //Navigation.PushAsync(new SlideFromTopView());
            //Navigation.PushAsync(new SlideFromLeftView());
            //Navigation.PushAsync(new SlideFromRightView());
        }
    }
}