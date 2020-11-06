using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms.Internals;

namespace SampleApp2.ViewModels
{
    public class ItemsViewModel
    {
        public ObservableCollection<string> Items { get; set; } = new ObservableCollection<string>();

        public ItemsViewModel()
        {
            Enumerable.Range(1, 10000).ForEach(i => Items.Add(i.ToString()));
        }
    }
}
