using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WorkshopScheduler.Views
{
    public partial class ToolbarView : NavigationPage
    {
        public ToolbarView()
        {
            InitializeComponent();

            this.ToolbarItems.Add(new ToolbarItem() { Icon = "user.png", });
            this.ToolbarItems.Add(new ToolbarItem() { Icon = "settings.png" });
            this.PushAsync(new MainView());
        }
    }
}
