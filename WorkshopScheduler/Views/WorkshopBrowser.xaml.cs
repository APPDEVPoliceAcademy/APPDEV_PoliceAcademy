using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using WorkshopScheduler.Models;
using WorkshopScheduler.RestLogic;


namespace WorkshopScheduler.Views
{
    public partial class WorkshopBrowser : ContentPage
    {

        List<WorkshopDTO> workshopsList;
        private RestService _restService;

        public WorkshopBrowser()
        {
            InitializeComponent();
            _restService = new RestService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            workshopsList = await _restService.GetAllWorkshopsAsync();
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

        private async void WorkshopsListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            var currentItem = e.SelectedItem as WorkshopDTO;
            await Navigation.PushModalAsync(new WorkshopDetail(currentItem.Id));
            WorkshopsListView.SelectedItem = null;
        }
    }
}
