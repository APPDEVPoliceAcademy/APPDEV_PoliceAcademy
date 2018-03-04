using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WorkshopScheduler.Views
{
    public partial class MenuPage : ContentPage
    {

        public MenuPage()
        {
            InitializeComponent();

            var masterPageItems = new List<MenuItem>();

            masterPageItems.Add(new MenuItem
            {
                Title = "Browser",
                IconSource = "contacts.png",
                TargetType = typeof(WorkshopBrowser)
            });

            masterPageItems.Add(new MenuItem
            {
                Title = "Reservations",
                IconSource = "todo.png",
                TargetType = typeof(WorkshopBrowser)
            });

            masterPageItems.Add(new MenuItem
            {
                Title = "My profile",
                IconSource = "todo.png",
                TargetType = typeof(WorkshopBrowser)
            });
            masterPageItems.Add(new MenuItem
            {
                Title = "Settings",
                IconSource = "reminders.png",
                TargetType = typeof(WorkshopBrowser)
            });

            menuListView.SeparatorVisibility = SeparatorVisibility.None;
            menuListView.ItemsSource = masterPageItems;
        }


        void ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MenuItem;
            if (item != null)
            {
            }
        }
    }
}
