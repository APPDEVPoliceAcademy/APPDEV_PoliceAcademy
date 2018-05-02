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
        Sorting sortings = new Sorting();
        Filters filters = new Filters();
        FiltersModalView filtersView;
        private RestService _restService;
        public event EventHandler<WorkshopDTO> UserEnrolled;
        public event EventHandler<WorkshopDTO> UserDisenrolled;
        public event EventHandler<WorkshopDTO> WorkshopEvaluated;


        public WorkshopBrowser()
        {
            InitializeComponent();

           
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
                displayList = filters.FilterBy12Weeks(workshopsList, flag);
                WorkshopsListView.ItemsSource = displayList;
            };

            filtersView.UnitFilterChanged += (o, place) =>
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


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await _restService.SaveFile("http://szgrabowski.kis.p.lodz.pl/zpo16/dla_chetnych/dna_seq.dat", "lab02.pdf");

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
                   // displayList = workshopsList;
                    ActivityIndicator.IsRunning = false;
                    ActivityIndicator.IsVisible = false;
                    WorkshopsListView.IsVisible = true;

                    //apply default sorting & display only future events
                    displayList = sortings.ByDateAscending(filters.FilterByDate(workshopsList, new DateTime[] { DateTime.Now, DateTime.Now.AddYears(1) }));
                   
                }

                WorkshopsListView.ItemsSource = displayList;
            }
            

        }

        private void SearchWorkshop_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (workshopsList == null)
                return;

            if (SearchWorkshop.Text != null)
            {
                WorkshopsListView.ItemsSource = displayList.Where(x =>
                                                x.Title.IndexOf(SearchWorkshop.Text.Trim(' '), 0, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();

            }
            else
            {
                displayList = sortings.ByDateAscending(filters.FilterByDate(workshopsList, new DateTime[] { DateTime.Now, DateTime.Now.AddYears(1) }));
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
                workshopsList = new ObservableCollection<WorkshopDTO>(workshopsResponse.Value);
               // displayList = workshopsList;
                WorkshopsListView.IsRefreshing = false;

                //apply default sorting & display only future events
                displayList = sortings.ByDateAscending(filters.FilterByDate(workshopsList, new DateTime[] { DateTime.Now, DateTime.Now.AddYears(1) }));

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
            workshopDetailpage.UserEnrolled += (o, workshop) =>
            {
                var workshopDto = workshopsList.FirstOrDefault(dto => dto.Id == workshop.Id);
                if (workshopDto != null)
                {
                    workshopDto.IsEnrolled = true;
                    workshopDto.TakenSpots++;
                }
                UserEnrolled?.Invoke(this, workshopDto);
            };
            workshopDetailpage.UserDisenrolled += (o, workshop) =>
            {
                var workshopDto = workshopsList.FirstOrDefault(dto => dto.Id == workshop.Id);
                if (workshopDto != null)
                {
                    workshopDto.IsEnrolled = false;
                    workshopDto.TakenSpots--;
                }
                UserDisenrolled?.Invoke(this, workshopDto);
            };
            workshopDetailpage.WorkshopEvaluated += (o, workshop) =>
            {
                var workshopDto = workshopsList.FirstOrDefault(dto => dto.Id == workshop.Id);
                if (workshopDto != null) workshopDto.IsEvaluated = true;
                WorkshopEvaluated?.Invoke(this, workshopDto);

            };
            await Navigation.PushModalAsync(workshopDetailpage);
            WorkshopsListView.SelectedItem = null;
        }

        //Handler For Disenrolling
        public void OnUserDisenrolled(object sender, WorkshopDTO workshopDto)
        {
            var workshop = workshopsList.FirstOrDefault(dto => dto.Id == workshopDto.Id);
            if (workshop != null)
            {
                workshop.IsEnrolled = false;
                workshop.TakenSpots--;
            }
        }

        //Handler for workshop evalauation from reserved browser
        public void OnWorkshopEvaluated(object sender, WorkshopDTO workshopDto)
        {
            var workshop = workshopsList.FirstOrDefault(dto => dto.Id == workshopDto.Id);
            if (workshop != null)
            {
                workshop.IsEvaluated = true;
            }
        }       
    }
}
