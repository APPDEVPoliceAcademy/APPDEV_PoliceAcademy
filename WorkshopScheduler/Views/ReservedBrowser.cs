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
        /*
        public struct SettingsToApply
        {
            public bool weeksFilter;
            public bool datesFilter;
            public DateTime[] dates;
            public bool unitFilter;
            public Unit unit;
            public SortingsEnum chosenSorting;

            public SettingsToApply(bool weeksFilter, bool datesFilter, DateTime[] dates, bool unitFilter, Unit unit)
            {
                this.weeksFilter = weeksFilter;
                this.datesFilter = datesFilter;
                this.dates = dates;
                this.unitFilter = unitFilter;
                this.unit = unit;
                this.chosenSorting = SortingsEnum.ByDateDescending;
            }
        }




        //ObservableCollection<WorkshopDTO> workshopListOnlyFuture;
        ObservableCollection<WorkshopDTO> workshopsRawList;
        ObservableCollection<WorkshopDTO> displayList;
        Sorting sortings = new Sorting();
        Filters filters = new Filters();
        FiltersModalView filtersView;
        private RestService _restService;
        public event EventHandler<WorkshopDTO> UserDisenrolled;
        public event EventHandler<WorkshopDTO> WorkshopEvaluated;
        Views.SettingsToApply chosenSettings;

        public ReservedBrowser()
        {
            InitializeComponent();

            //We are using only one filtersView, therefore when we initialize we can set date filter
            filtersView = new FiltersModalView();
            chosenSettings = new Views.SettingsToApply(false, true,
                    new DateTime[] {DateTime.Now, DateTime.Now.AddYears(2)},
                    false, Unit.None)
                {chosenSorting = SortingsEnum.ByDateAscending};

            //When user closes filters, update listview
            filtersView.IsFinished += (o, b) => { displayList = ApplySorting(ApplyFilters(workshopsRawList)); };

            filtersView.SortingChanged += (o, sortingChosen) => { chosenSettings.chosenSorting = sortingChosen; };

            filtersView.DatesFilterChanged += (o, args) =>
            {

                chosenSettings.datesFilter = args.isOn;
                if (chosenSettings.datesFilter)
                    chosenSettings.dates = args.dates;


            };

            filtersView.WeeksFilterChanged += (o, flag) =>
            {
                chosenSettings.weeksFilter = (chosenSettings.weeksFilter == true) ? false : true;
            };

            filtersView.UnitFilterChanged += (o, place) =>
            {
                chosenSettings.unitFilter = (chosenSettings.weeksFilter == true) ? false : true;
            };


            _restService = new RestService();
        }

        private ObservableCollection<WorkshopDTO> ApplyFilters(ObservableCollection<WorkshopDTO> _displayList)
        {
            if (chosenSettings.datesFilter)
                _displayList = filters.FilterByDate(_displayList, chosenSettings.dates);



            if (chosenSettings.weeksFilter)
                _displayList = filters.FilterBy12Weeks(_displayList);


            if (chosenSettings.unitFilter)
                _displayList = filters.FilterByPlace(_displayList, chosenSettings.unit.ToString());


            return _displayList;
        }

        private ObservableCollection<WorkshopDTO> ApplySorting(ObservableCollection<WorkshopDTO> _displaylist)
        {

            switch (chosenSettings.chosenSorting)
            {
                case SortingsEnum.ByDateAscending:
                    _displaylist = sortings.ByDateAscending(_displaylist);
                    break;
                case SortingsEnum.ByDateDescending:
                    _displaylist = sortings.ByDateDescending(_displaylist);
                    break;
                case SortingsEnum.ByTitleAscending:
                    _displaylist = sortings.ByTitleAscending(_displaylist);
                    break;
                case SortingsEnum.ByTitleDescending:
                    _displaylist = sortings.ByTitleDescending(_displaylist);
                    break;
                //case SortingsEnum.None:
                //_displaylist = workshopListOnlyFuture;
                //break;
                default:
                    //DisplayAlert("couldn't match any", "", "ok");
                    break;
            }

            ;

            return _displaylist;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();



            if (workshopsRawList == null)
            {
                var workshopsResponse = await _restService.GetUserWorkshopAsync();

                if (workshopsResponse.ResponseCode == null)
                {

                    await DisplayAlert("Error",
                        workshopsResponse.ErrorMessage + "\nMake sure that you have internet connection", "Ok");
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
                    workshopsRawList = new ObservableCollection<WorkshopDTO>(workshopsResponse.Value);
                    displayList = ApplySorting(ApplyFilters(workshopsRawList));



                    ActivityIndicator.IsRunning = false;
                    ActivityIndicator.IsVisible = false;
                    WorkshopsListView.IsVisible = true;


                }

            }

            WorkshopsListView.ItemsSource = displayList;
        }

        private void SearchWorkshop_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (displayList == null)
                return;

            if (SearchWorkshop.Text != null)
            {
                WorkshopsListView.ItemsSource = displayList.Where(x =>
                        x.Title.IndexOf(SearchWorkshop.Text.Trim(' '), 0, StringComparison.CurrentCultureIgnoreCase) !=
                        -1)
                    .ToList();

            }
            else
            {
                //displayList = sortings.ByDateAscending(workshopListOnlyFuture);
                WorkshopsListView.ItemsSource = displayList;
            }
        }

        private async void WorkshopsListView_OnRefreshing(object sender, EventArgs e)
        {
            //Check if there is any point in refreshing !!!


            var workshopsResponse = await _restService.GetAllWorkshopsAsync();

            if (workshopsResponse.ResponseCode == null)
            {
                await DisplayAlert("Error",
                    workshopsResponse.ErrorMessage + "\nMake sure that you have internet connection", "Ok");
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
                workshopsRawList = new ObservableCollection<WorkshopDTO>(workshopsResponse.Value);
                displayList = ApplySorting(ApplyFilters(workshopsRawList));

                //workshopListOnlyFuture = filters.FilterByDate(workshopsRawList, new DateTime[] { DateTime.Now.Date, DateTime.Now.Date.AddYears(1) }); //we are working only on future events here!

                WorkshopsListView.IsRefreshing = false;

                //apply default sorting 
                //displayList = sortings.ByDateAscending(workshopListOnlyFuture);

            }

            WorkshopsListView.ItemsSource = displayList;
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
            var workshopDetailpage = new WorkshopDetail(currentItem.Id);
        
            workshopDetailpage.UserDisenrolled += (o, workshop) =>
            {
                var workshopDto = workshopsRawList.FirstOrDefault(dto => dto.Id == workshop.Id);
                if (workshopDto != null)
                {
                    workshopDto.IsEnrolled = false;
                    workshopDto.TakenSpots--;
                }

                UserDisenrolled?.Invoke(this, workshopDto);
            };
            workshopDetailpage.WorkshopEvaluated += (o, workshop) =>
            {
                //Update raw list
                var workshopDto = workshopsRawList.FirstOrDefault(dto => dto.Id == workshop.Id);
                if (workshopDto != null) workshopDto.IsEvaluated = true;
                WorkshopEvaluated?.Invoke(this, workshopDto);

            };
            await Navigation.PushModalAsync(workshopDetailpage);
            WorkshopsListView.SelectedItem = null;
        }

        //Handler For Disenrolling
        public void OnUserDisenrolled(object sender, WorkshopDTO workshopDto)
        {
            var workshop = workshopsRawList.FirstOrDefault(dto => dto.Id == workshopDto.Id);
            if (workshop != null)
            {
                workshop.IsEnrolled = false;
                workshop.TakenSpots--;
            }
        }

        //Handler for workshop evalauation from reserved browser
        public void OnWorkshopEvaluated(object sender, WorkshopDTO workshopDto)
        {
            var workshop = workshopsRawList.FirstOrDefault(dto => dto.Id == workshopDto.Id);
            if (workshop != null)
            {
                workshop.IsEvaluated = true;
            }
        }

        public void OnUserEnrolled(object sender, WorkshopDTO workshopDto)
        {
            var workshop = workshopsRawList.FirstOrDefault(dto => dto.Id == workshopDto.Id);
            if (workshop != null)
            {
                workshop.IsEnrolled = true;
            }
        }
        */
    }

}