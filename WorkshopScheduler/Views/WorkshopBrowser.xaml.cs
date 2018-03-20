using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using WorkshopScheduler.Models;



namespace WorkshopScheduler.Views
{
    public partial class WorkshopBrowser : ContentPage
    {

        List<Workshop> workshopsList;

        public WorkshopBrowser()
        {
            InitializeComponent();
          
            workshopsList = TestData.LoremIpsumData; // provide the test data
            WorkshopsListView.ItemsSource = workshopsList; 
        }
        private void SearchWorkshop_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SearchWorkshop.Text != null)
            {
                var displayList = workshopsList.Where(x =>
                    x.Title.IndexOf(SearchWorkshop.Text.Trim(' '), 0, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();

                WorkshopsListView.ItemsSource = displayList;
            }
            else
            {
                WorkshopsListView.ItemsSource = workshopsList;
            }
        }
    }
}
