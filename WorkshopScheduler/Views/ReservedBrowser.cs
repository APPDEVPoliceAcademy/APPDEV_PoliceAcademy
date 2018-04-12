using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using WorkshopScheduler;
using WorkshopScheduler.Logic;
using Xamarin.Forms;
using WorkshopScheduler.Models;
using WorkshopScheduler.RestLogic;
using WorkshopScheduler.Views;
using WorkshopScheduler.Views.UserAccountViews;

namespace WorkshopScheduler.Views
{
    public partial class ReservedBrowser : ContentPage
    {
        ObservableCollection<WorkshopDTO> reservedList;
        ObservableCollection<WorkshopDTO> displayList;
        FiltersModalView filtersView;
        private RestService _restService;

        public event EventHandler<WorkshopDTO> UserDisenrolled;


        public ReservedBrowser()
        {
            InitializeComponent();
            //lorem ipsum is to test longer strings, but I dont want to deal with them normally ;) 

            Sorting sortings = new Sorting();
            Filters filters = new Filters();

            filtersView = new FiltersModalView();

            filtersView.SortingChanged += (o, sortingChosen) =>
            {
                //displayList = null;

                switch (sortingChosen)
                {
                    case SortingsEnum.ByDateAscending:
                        displayList = sortings.ByDateAscending(reservedList);
                        break;
                    case SortingsEnum.ByDateDescending:
                        displayList = sortings.ByDateDescending(reservedList);
                        break;
                    case SortingsEnum.ByTitleAscending:
                        displayList = sortings.ByTitleAscending(reservedList);
                        break;
                    case SortingsEnum.ByTitleDescending:
                        displayList = sortings.ByTitleDescending(reservedList);
                        break;
                    case SortingsEnum.None:
                        displayList = reservedList;
                        break;
                    default:
                        //DisplayAlert("couldn't match any", "shit", "ok");
                        break;
                };


                WorkshopsListView.ItemsSource = displayList;

            };

            filtersView.DatesFilterChanged += (o, dates) =>
            {
                displayList = filters.FilterByDate(reservedList, dates);
                WorkshopsListView.ItemsSource = displayList;
            };

            filtersView.WeeksFilterChanged += (o, flag) =>
            {
                displayList = filters.FilterBy12weeks(reservedList, flag);
                WorkshopsListView.ItemsSource = displayList;
            };

            filtersView.UnitFilterChanged += (o, place) =>
            {
                displayList = filters.FilterByPlace(reservedList, place.ToString());
                WorkshopsListView.ItemsSource = displayList;
            };

            filtersView.ResetSettings += (o, s) => {
                WorkshopsListView.ItemsSource = reservedList;
            };
            _restService = new RestService();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (reservedList == null)
            {
                var workshopsResponse = await _restService.GetUserWorkshopAsynch();

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
                    reservedList = new ObservableCollection<WorkshopDTO>(workshopsResponse.Value);
                }
                WorkshopsListView.ItemsSource = reservedList;
            }
            

        }

         void SearchWorkshop_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SearchWorkshop.Text != null)
            {
                WorkshopsListView.ItemsSource = displayList.Where(x =>
                                                x.Title.IndexOf(SearchWorkshop.Text.Trim(' '), 0, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();

            }
            else
            {
                WorkshopsListView.ItemsSource = reservedList;
            }
        }

        private async void WorkshopsListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            var currentItem = e.SelectedItem as WorkshopDTO;
            var workshopDetailpage = new WorkshopDetail(currentItem.Id);
            workshopDetailpage.UserDisenrolled += (o, workshop) =>
            {
                var workshopDto = reservedList.FirstOrDefault(dto => dto.Id == workshop.Id);
                reservedList.Remove(workshopDto);
                UserDisenrolled.Invoke(this, workshopDto);
            };
            await Navigation.PushModalAsync(workshopDetailpage);
            WorkshopsListView.SelectedItem = null;
        }

        async void SortingsButton_OnClicked(object sender, System.EventArgs e)
        {
            // DisplayAlert("refreshed",sortingChosen.ToString(),"ok");

            await Navigation.PushModalAsync(filtersView);

        }


        //Handler for event send from WorkshopBrowser, when user enrolls on given workshop
        public void OnWorkshopEnrolled(object sender, WorkshopDTO workshopDto)
        {
            reservedList?.Add(workshopDto);
        }
    }

}