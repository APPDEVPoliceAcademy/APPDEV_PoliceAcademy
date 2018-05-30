using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Plugin.Messaging;
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
            this.chosenSorting = SortingsEnum.DatumDalend;
        }
    }

    public partial class WorkshopBrowser : ContentPage
    {


        //ObservableCollection<WorkshopDTO> workshopListOnlyFuture;
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
        private WorkshopBrowserType _type;

        public WorkshopBrowser(char type)
        {
            InitializeComponent();
            if (type == 'a')
            {
                _type = WorkshopBrowserType.All;
            }
            else
            {
                _type = WorkshopBrowserType.Reserved;
            }

            //We are using only one filtersView, therefore when we initialize we can set date filter
            filtersView = new FiltersModalView(_type);
            if (_type == WorkshopBrowserType.All)
            {
                chosenSettings = new SettingsToApply(false, true, new DateTime[] { DateTime.Now, DateTime.Now.AddYears(2) },
                        false, Unit.None)
                    { chosenSorting = SortingsEnum.DatumStijgend };
            }
            else
            {
                
                chosenSettings = new SettingsToApply(false, true , new DateTime[] { DateTime.Now.AddYears(-5), DateTime.Now.AddYears(2) },
                        false, Unit.None)
                    { chosenSorting = SortingsEnum.DatumStijgend };
            }
            

            //When user closes filters, update listview
            filtersView.IsFinished += (o, b) =>
            {
                displayList = ApplySorting(ApplyFilters(workshopsRawList));
            };

            filtersView.SortingChanged += (o, sortingChosen) =>
            {

                chosenSettings.chosenSorting = sortingChosen;

            };

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

        private ObservableCollection<WorkshopDTO> ApplyFilters( ObservableCollection<WorkshopDTO>  _displayList)
        {
            if (chosenSettings.datesFilter)
                _displayList = filters.FilterByDate(_displayList, chosenSettings.dates);
            
            if (chosenSettings.weeksFilter)
                _displayList = filters.FilterBy12Weeks(_displayList);

          
            if (chosenSettings.unitFilter)
                _displayList = filters.FilterByPlace(_displayList, chosenSettings.unit.ToString());


          return _displayList;
        }

        private ObservableCollection<WorkshopDTO> ApplySorting( ObservableCollection<WorkshopDTO> _displaylist)
        {

            switch (chosenSettings.chosenSorting)
            {
                case SortingsEnum.DatumStijgend:
                    _displaylist = sortings.ByDateAscending(_displaylist);
                    break;
                case SortingsEnum.DatumDalend:
                    _displaylist = sortings.ByDateDescending(_displaylist);
                    break;
                case SortingsEnum.TitelStijgend:
                    _displaylist = sortings.ByTitleAscending(_displaylist);
                    break;
                case SortingsEnum.TitelDalend:
                    _displaylist = sortings.ByTitleDescending(_displaylist);
                    break;
                default:
                    break;
            };

            return _displaylist;
        }
      

        protected override async void OnAppearing()
        {
            base.OnAppearing();

           

            if (workshopsRawList == null)
            {
                
                RestResponse<List<WorkshopDTO>> workshopsResponse;

                if (_type == WorkshopBrowserType.All)
                {
                    workshopsResponse = await _restService.GetAllWorkshopsAsync();
                }
                else
                {
                    workshopsResponse = await _restService.GetUserWorkshopAsync();  
                }
                    
                    

                if (workshopsResponse.ResponseCode == null)
                {

                    await DisplayAlert("Error", workshopsResponse.ErrorMessage + "\nControleer of u verbining heeft met het internet", "Ok");
                    WorkshopsListView.ItemsSource = new ObservableCollection<WorkshopDTO>();
                }

                if (workshopsResponse.ResponseCode == HttpStatusCode.Unauthorized)
                {
                    //Check token validation additionaly
                    await DisplayAlert("Error", "Uw sessie is verlopen, U wordt terug gestuurd naar de login pagina", "Ok");
                    WorkshopsListView.ItemsSource = new ObservableCollection<WorkshopDTO>();
                    Application.Current.MainPage = new LoginView();
                }

                if (workshopsResponse.ResponseCode == HttpStatusCode.NotFound)
                {
                    workshopsRawList = new ObservableCollection<WorkshopDTO>();
                    ActivityIndicator.IsRunning = false;
                    ActivityIndicator.IsVisible = false;
                    WorkshopsListView.IsVisible = true;
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
                                                x.Title.IndexOf(SearchWorkshop.Text.Trim(' '), 0, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();

            }
            else
            {
                WorkshopsListView.ItemsSource = displayList;
            }
        }

        private async void WorkshopsListView_OnRefreshing(object sender, EventArgs e)
        {
            //Check if there is any point in refreshing !!!


            RestResponse<List<WorkshopDTO>> workshopsResponse;

            if (_type == WorkshopBrowserType.All)
            {
                workshopsResponse = await _restService.GetAllWorkshopsAsync();
            }
            else
            {
                workshopsResponse = await _restService.GetUserWorkshopAsync();
            }

            if (workshopsResponse.ResponseCode == null)
            {
                await DisplayAlert("Error", workshopsResponse.ErrorMessage + "\nControleer of u verbining heeft met het internet", "Ok");
                WorkshopsListView.ItemsSource = new ObservableCollection<WorkshopDTO>();
            }

            if (workshopsResponse.ResponseCode == HttpStatusCode.Unauthorized)
            {
                //Check token validation additionaly
                await DisplayAlert("Error", "Uw sessie is verlopen, U wordt terug gestuurd naar de login pagina", "Ok");
                WorkshopsListView.ItemsSource = new ObservableCollection<WorkshopDTO>();
                Application.Current.MainPage = new LoginView();
            }

            if (workshopsResponse.ResponseCode == HttpStatusCode.OK)
            {
                workshopsRawList = new ObservableCollection<WorkshopDTO>(workshopsResponse.Value);
                displayList = ApplySorting(ApplyFilters(workshopsRawList));
                WorkshopsListView.IsRefreshing = false;
            }

            WorkshopsListView.ItemsSource = displayList;
        }

        async void SortingsButton_OnClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(filtersView);
        }

        private async void WorkshopsListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            var currentItem = e.SelectedItem as WorkshopDTO;
            var workshopDetailpage = new WorkshopDetail(currentItem.Id);
            workshopDetailpage.UserEnrolled += (o, workshop) =>
            {
                //This can be called only from workshop all
                //Update raw list
                var workshopDto = workshopsRawList.FirstOrDefault(dto => dto.Id == workshop.Id);
                if (workshopDto != null)
                {
                    workshopDto.IsEnrolled = true;
                    workshopDto.TakenSpots++;
                }
                UserEnrolled?.Invoke(this, workshopDto);
            };
            workshopDetailpage.UserDisenrolled += (o, workshop) =>
            {
                var workshopDto = workshopsRawList.FirstOrDefault(dto => dto.Id == workshop.Id);
                if (workshopDto != null)
                {
                    if (_type == WorkshopBrowserType.All)
                    {
                        workshopDto.IsEnrolled = false;
                        workshopDto.TakenSpots--;
                    } else if (_type == WorkshopBrowserType.Reserved)
                    {
                        workshopsRawList.Remove(workshopDto);
                        displayList = ApplySorting(ApplyFilters(workshopsRawList));
                    }

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
            var workshop = workshopsRawList?.FirstOrDefault(dto => dto.Id == workshopDto.Id);
            if (workshop == null) return;
            if (_type == WorkshopBrowserType.All)
            {
                    workshop.IsEnrolled = false;
                    workshop.TakenSpots--;
            }
            else
            {
                workshopsRawList.Remove(workshop);
                displayList = ApplySorting(ApplyFilters(workshopsRawList));
            }
            
        }

        //Handler for workshop evalauation from reserved browser
        public void OnWorkshopEvaluated(object sender, WorkshopDTO workshopDto)
        {
            var workshop = workshopsRawList?.FirstOrDefault(dto => dto.Id == workshopDto.Id);
            if (workshop != null)
            {
                workshop.IsEvaluated = true;
            }
        }

        public void OnUserEnrolled(object sender, WorkshopDTO workshopDto)
        {
            if (_type == WorkshopBrowserType.Reserved)
            {
                workshopsRawList?.Add(workshopDto);
                displayList = ApplySorting(ApplyFilters(workshopsRawList));
                return;
            }
            else
            {
                var workshop = workshopsRawList?.FirstOrDefault(dto => dto.Id == workshopDto.Id);

                if (workshop == null) return;
                workshop.TakenSpots++;
                workshop.IsEnrolled = true;
            }
                
            
        }

        private void IdeaButton_OnClicked(object sender, EventArgs e)
        {
            var emailMessenger = CrossMessaging.Current.EmailMessenger;

            if (emailMessenger.CanSendEmail)
            {
                var email = new EmailMessageBuilder()
                    .To("inspiratiepunt@politieacademie.nl")
                    .Subject("Ik heb een Idee")
                    .BodyAsHtml("Omschrijf hieronder het idee dat u heeft voor de workshop")
                    .Build();
                Task.Delay(100);
                emailMessenger.SendEmail(email);
            }
            else
            {
                DisplayAlert("Whoops!", "Dit apparaad kan geen E-mails verzenden", "ok");
            }
        }
    }
}


 