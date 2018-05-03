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

    public partial class WorkshopBrowser : ContentPage
    {


        ObservableCollection<WorkshopDTO> workshopListOnlyFuture;
        ObservableCollection<WorkshopDTO> workshopsRawList;
        ObservableCollection<WorkshopDTO> displayList;
        Sorting sortings = new Sorting();
        Filters filters = new Filters();
        FiltersModalView filtersView;
        private RestService _restService;
        public event EventHandler<WorkshopDTO> UserEnrolled;
        public event EventHandler<WorkshopDTO> UserDisenrolled;
        public event EventHandler<WorkshopDTO> WorkshopEvaluated;
        SettingsToApply chosenSettings;

        public WorkshopBrowser()
        {
            InitializeComponent();


            filtersView = new FiltersModalView();
            chosenSettings = new SettingsToApply(false, false, null, false, Unit.None);


            filtersView.SortingChanged += (o, sortingChosen) =>
            {
                
                WorkshopsListView.ItemsSource = applySorting(displayList,sortingChosen);

            };

            filtersView.DatesFilterChanged += (o, dates) =>
            {
                //if(dates[0] < DateTime.Now.Date)
                //    displayList = filters.FilterByDate(workshopsRawList, dates);
                //else
                //    displayList = filters.FilterByDate(displayList, dates);

                //WorkshopsListView.ItemsSource = displayList;
                chosenSettings.datesFilter = (chosenSettings.datesFilter == true) ? false : true;
                if (chosenSettings.datesFilter)
                    chosenSettings.dates = dates;
               

            };

            filtersView.WeeksFilterChanged += (o, flag) =>
            {
                //displayList = filters.FilterBy12Weeks(displayList, flag);
                //WorkshopsListView.ItemsSource = displayList;
                chosenSettings.weeksFilter = (chosenSettings.weeksFilter == true) ? false : true;
            };

            filtersView.UnitFilterChanged += (o, place) =>
            {
                //displayList = filters.FilterByPlace(displayList, place.ToString());
                //WorkshopsListView.ItemsSource = displayList;
                chosenSettings.unitFilter = (chosenSettings.weeksFilter == true) ? false : true;
            };

            filtersView.ResetSettings += (o, s) =>
            {
                //displayList = workshopListOnlyFuture;
                //WorkshopsListView.ItemsSource = displayList;
                chosenSettings = new SettingsToApply(false, false, null, false, Unit.None); //just to remove all filters
            };

            _restService = new RestService();
        }

        private ObservableCollection<WorkshopDTO> applyFilters( ObservableCollection<WorkshopDTO>  _displayList)
        {
            if (chosenSettings.datesFilter)
                _displayList = filters.FilterByDate(_displayList, chosenSettings.dates);
            else
                _displayList = filters.FilterByDate(workshopsRawList, new DateTime[] { DateTime.Now.Date, DateTime.Now.Date.AddYears(1) });


            if (chosenSettings.weeksFilter)
                _displayList = filters.FilterBy12Weeks(_displayList);

          
            if (chosenSettings.unitFilter)
                _displayList = filters.FilterByPlace(_displayList, chosenSettings.unit.ToString());


          return _displayList;
        }

        private ObservableCollection<WorkshopDTO> applySorting( ObservableCollection<WorkshopDTO> _displaylist, SortingsEnum sortingChosen)
        {

            switch (sortingChosen)
            {
                case SortingsEnum.ByDateAscending:
                    _displaylist = sortings.ByDateAscending(displayList);
                    break;
                case SortingsEnum.ByDateDescending:
                    _displaylist = sortings.ByDateDescending(displayList);
                    break;
                case SortingsEnum.ByTitleAscending:
                    _displaylist = sortings.ByTitleAscending(displayList);
                    break;
                case SortingsEnum.ByTitleDescending:
                    _displaylist = sortings.ByTitleDescending(displayList);
                    break;
                case SortingsEnum.None:
                    _displaylist = workshopListOnlyFuture;
                    break;
                default:
                    //DisplayAlert("couldn't match any", "", "ok");
                    break;
            };

            return _displaylist;
        }
       

        protected override async void OnAppearing()
        {
            base.OnAppearing();

           

            if (workshopsRawList == null)
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
                    workshopsRawList = new ObservableCollection<WorkshopDTO>(workshopsResponse.Value);
                    workshopListOnlyFuture = filters.FilterByDate(workshopsRawList, new DateTime[] { DateTime.Now.Date, DateTime.Now.Date.AddYears(1) }); //we are working only on future events here!


                    ActivityIndicator.IsRunning = false;
                    ActivityIndicator.IsVisible = false;
                    WorkshopsListView.IsVisible = true;

                    //apply default sorting
                    displayList = applySorting(workshopListOnlyFuture, chosenSettings.chosenSorting);

                }


                WorkshopsListView.ItemsSource = applyFilters(displayList);
            }


        }

        private void SearchWorkshop_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (workshopListOnlyFuture == null)
                return;

            if (SearchWorkshop.Text != null)
            {
                WorkshopsListView.ItemsSource = displayList.Where(x =>
                                                x.Title.IndexOf(SearchWorkshop.Text.Trim(' '), 0, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();

            }
            else
            {
                displayList = sortings.ByDateAscending(workshopListOnlyFuture);
                WorkshopsListView.ItemsSource = displayList;
            }
        }

        private async void WorkshopsListView_OnRefreshing(object sender, EventArgs e)
        {
            //Check if there is any point in refreshing !!!


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
                workshopsRawList = new ObservableCollection<WorkshopDTO>(workshopsResponse.Value);
                workshopListOnlyFuture = filters.FilterByDate(workshopsRawList, new DateTime[] { DateTime.Now.Date, DateTime.Now.Date.AddYears(1) }); //we are working only on future events here!

                WorkshopsListView.IsRefreshing = false;

                //apply default sorting 
                //displayList = sortings.ByDateAscending(workshopListOnlyFuture);

            }

            WorkshopsListView.ItemsSource = applySorting(applyFilters(workshopListOnlyFuture),chosenSettings.chosenSorting);
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
            workshopDetailpage.UserEnrolled += (o, workshop) =>
            {
                var workshopDto = workshopListOnlyFuture.FirstOrDefault(dto => dto.Id == workshop.Id);
                if (workshopDto != null)
                {
                    workshopDto.IsEnrolled = true;
                    workshopDto.TakenSpots++;
                }
                UserEnrolled?.Invoke(this, workshopDto);
            };
            workshopDetailpage.UserDisenrolled += (o, workshop) =>
            {
                var workshopDto = workshopListOnlyFuture.FirstOrDefault(dto => dto.Id == workshop.Id);
                if (workshopDto != null)
                {
                    workshopDto.IsEnrolled = false;
                    workshopDto.TakenSpots--;
                }
                UserDisenrolled?.Invoke(this, workshopDto);
            };
            workshopDetailpage.WorkshopEvaluated += (o, workshop) =>
            {
                var workshopDto = workshopListOnlyFuture.FirstOrDefault(dto => dto.Id == workshop.Id);
                if (workshopDto != null) workshopDto.IsEvaluated = true;
                WorkshopEvaluated?.Invoke(this, workshopDto);

            };
            await Navigation.PushModalAsync(workshopDetailpage);
            WorkshopsListView.SelectedItem = null;
        }

        //Handler For Disenrolling
        public void OnUserDisenrolled(object sender, WorkshopDTO workshopDto)
        {
            var workshop = workshopListOnlyFuture.FirstOrDefault(dto => dto.Id == workshopDto.Id);
            if (workshop != null)
            {
                workshop.IsEnrolled = false;
                workshop.TakenSpots--;
            }
        }

        //Handler for workshop evalauation from reserved browser
        public void OnWorkshopEvaluated(object sender, WorkshopDTO workshopDto)
        {
            var workshop = workshopListOnlyFuture.FirstOrDefault(dto => dto.Id == workshopDto.Id);
            if (workshop != null)
            {
                workshop.IsEvaluated = true;
            }
        }
    }
}
