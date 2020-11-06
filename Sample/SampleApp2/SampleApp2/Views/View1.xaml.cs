using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.NavView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApp2.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class View1 : XContentView
    {
        public View1()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            Debug.WriteLine($"{GetType()} Appearing");
        }

        protected override void OnDisappearing()
        {
            Debug.WriteLine($"{GetType()} Disappearing");
        }
    }
}