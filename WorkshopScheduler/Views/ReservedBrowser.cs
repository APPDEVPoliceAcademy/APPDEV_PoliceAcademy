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
    public partial class ReservedBrowser : ContentPage
    {
        List<WorkshopDTO> reservedList;
        private IRestService _restService;

        public ReservedBrowser()
        {
            InitializeComponent();
            _restService = new RestService();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            reservedList = await _restService.GetUserWorkshopAsynch();
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