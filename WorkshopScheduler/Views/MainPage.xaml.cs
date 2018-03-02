using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WorkshopScheduler.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        void browserChosen(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new WorkshopBrowser());
        }
    }
}
