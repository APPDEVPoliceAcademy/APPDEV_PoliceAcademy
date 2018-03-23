using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using WorkshopScheduler.Models;


namespace WorkshopScheduler.Views
{
    public partial class ReservedBrowser : ContentPage
    {
        ObservableCollection<Workshop> reservedList;

        public ReservedBrowser()
        {
            InitializeComponent();
            //lorem ipsum is to test longer strings, but I dont want to deal with them normally ;) 

            reservedList = TestData.LoremIpsumData; // provide the test

            WorkshopsListView.ItemsSource = reservedList;
        }


        private void SearchWorkshop_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SearchWorkshop.Text != null)
            {
                var displayList = reservedList.Where(x =>
                    x.Title.IndexOf(SearchWorkshop.Text.Trim(' '), 0, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();
                WorkshopsListView.ItemsSource = displayList;
            }
            else
            {
                WorkshopsListView.ItemsSource = reservedList;
            }
        }
    }
}