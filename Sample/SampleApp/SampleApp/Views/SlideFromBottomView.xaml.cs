﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xam.NavigationView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SampleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SlideFromBottomView : DefaultView
    {
        public SlideFromBottomView()
        {
            InitializeComponent();
        }
    }
}