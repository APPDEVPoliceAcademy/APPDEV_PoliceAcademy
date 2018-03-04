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
                Title = "Contacts",
                IconSource = "contacts.png",
                TargetType = typeof(WorkshopBrowser)
            });
            masterPageItems.Add(new MenuItem
            {
                Title = "TodoList",
                IconSource = "todo.png",
                TargetType = typeof(WorkshopBrowser)
            });
            masterPageItems.Add(new MenuItem
            {
                Title = "Reminders",
                IconSource = "reminders.png",
                TargetType = typeof(WorkshopBrowser)
            });

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
