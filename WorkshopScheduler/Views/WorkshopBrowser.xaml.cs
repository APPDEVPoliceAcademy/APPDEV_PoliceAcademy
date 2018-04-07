using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using Xamarin.Forms;
using WorkshopScheduler.Models;
using WorkshopScheduler.Logic;
using WorkshopScheduler.RestLogic;
using WorkshopScheduler.Views.UserAccountViews;


namespace WorkshopScheduler.Views
{
    public partial class WorkshopBrowser : ContentPage
    {

  
        ObservableCollection<WorkshopDTO> workshopsList;
        ObservableCollection<WorkshopDTO> displayList;
        FiltersModalView filtersView;
        private RestService _restService;


        public WorkshopBrowser()
        {
            InitializeComponent();

            Sorting sortings = new Sorting();
            Filters filters = new Filters();
            filtersView = new FiltersModalView();

            filtersView.SortingChanged += (o, sortingChosen) =>
            {
               // displayList = null;

                switch (sortingChosen)
                {
                    case SortingsEnum.ByDateAscending:
                        displayList = sortings.ByDateAscending(workshopsList);
                        break;
                    case SortingsEnum.ByDateDescending:
                        displayList = sortings.ByDateDescending(workshopsList);
                        break;
                    case SortingsEnum.ByTitleAscending:
                        displayList = sortings.ByTitleAscending(workshopsList);
                        break;
                    case SortingsEnum.ByTitleDescending:
                        displayList = sortings.ByTitleDescending(workshopsList);
                        break;
                    case SortingsEnum.None:
                        displayList = workshopsList;
                        break;
                    default:
                        //DisplayAlert("couldn't match any", "", "ok");
                        break;
                };


                WorkshopsListView.ItemsSource = displayList;

            };

            filtersView.DatesFilterChanged += (o, dates) =>
            {
                displayList = filters.FilterByDate(workshopsList, dates);
                WorkshopsListView.ItemsSource = displayList;
                
            };

            filtersView.WeeksFilterChanged += (o, flag) =>
            {
                displayList = filters.FilterBy12weeks(workshopsList, flag);
                WorkshopsListView.ItemsSource = displayList;
            };

            filtersView.PlaceFilterChanged += (o, place) =>
            {
                displayList = filters.FilterByPlace(workshopsList, place.ToString());
                WorkshopsListView.ItemsSource = displayList;
            };

            filtersView.ResetSettings += (o, s) => {
                displayList = workshopsList;
                WorkshopsListView.ItemsSource = displayList;
            };

            _restService = new RestService();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (workshopsList == null)
            {
                var workshopsResponse = await _restService.GetAllWorkshopsAsync();

                if (workshopsResponse.ResponseCode == null)
                {
                    await DisplayAlert("Error", workshopsResponse.ErrorMessage + "\nMake sure that you have internet connection", "Ok");
                    WorkshopsListView.ItemsSource = new ObservableCollection<WorkshopDTO>();
                }

                if (workshopsResponse.ResponseCode == HttpStatusCode.Unauthorized)
                {
                    //Check token validation additionaly
                    await DisplayAlert("Error", "Your session has expired. You will be redirected to log in", "Ok");
                    WorkshopsListView.ItemsSource = new ObservableCollection<WorkshopDTO>();
                    Application.Current.MainPage = new LoginView();
                }

                if (workshopsResponse.ResponseCode == HttpStatusCode.OK)
                {
                    workshopsList = new ObservableCollection<WorkshopDTO>(workshopsResponse.Value);
                }
                WorkshopsListView.ItemsSource = workshopsList;
            }
            

        }

        private void SearchWorkshop_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SearchWorkshop.Text != null)
            {
                WorkshopsListView.ItemsSource = displayList.Where(x =>
                                                x.Title.IndexOf(SearchWorkshop.Text.Trim(' '), 0, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();

            }
            else
            {
                WorkshopsListView.ItemsSource = workshopsList;
            }
        }

        async void SortingsButton_OnClicked(object sender, System.EventArgs e)
        {
            // DisplayAlert("refreshed",sortingChosen.ToString(),"ok");

            await Navigation.PushModalAsync(filtersView);

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
