using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WorkshopScheduler.Views
{
    public partial class HomePage : MasterDetailPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        void browserChosen(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new MainView());
        }
    }
}
