using System.Collections.Generic;
using Xamarin.Forms;

namespace WorkshopScheduler.Views
{
    public partial class MenuPage : ContentPage
    {

        public MenuPage()
        {
            InitializeComponent();

            var masterPageItems = new List<MenuItem>()
            {
                new MenuItem
                {
                    Title = "Main",
                    IconSource = "search.png",
                    TargetType = typeof(MainView)
                },
                new MenuItem
                {
                    Title = "My profile",
                    IconSource = "user.png",
                    TargetType = typeof(WorkshopBrowser)
                },
                new MenuItem
                {
                    Title = "Settings",
                    IconSource = "settings.png",
                    TargetType = typeof(WorkshopBrowser)
                }
            };
            MenuListView.SeparatorVisibility = SeparatorVisibility.None;
            MenuListView.ItemsSource = masterPageItems;
        }


        private void ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MenuItem;
            if (item != null)
            {
            }
        }
    }
}
