using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using WorkshopScheduler.Models;
using WorkshopScheduler.Logic;
using WorkshopScheduler.RestLogic;


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
                displayList = null;

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
                        DisplayAlert("couldn't match any", "shit", "ok");
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
                WorkshopsListView.ItemsSource = workshopsList;
            };
            _restService = new RestService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (workshopsList == null)
            {
                workshopsList = new ObservableCollection<WorkshopDTO>(await _restService.GetAllWorkshopsAsync());
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
