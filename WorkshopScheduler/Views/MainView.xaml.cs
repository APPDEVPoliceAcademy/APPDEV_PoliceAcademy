using System;
using System.Net;
using System.Net.Http;
using Plugin.Badge.Abstractions;
using WorkshopScheduler.Models;
using WorkshopScheduler.RestLogic;
using Xamarin.Forms;

namespace WorkshopScheduler.Views
{
    public partial class MainView : TabbedPage
    {
        public int Count { get; set; }
        private bool _onAppearingCalled = false;

        IRestService _restService = new RestService();
        public MainView()
        {
            
            InitializeComponent();


            //When user does something on workshopBrowser, update reservedBrowser
            WorkshopBrowser.UserEnrolled += ReservedBrowser.OnUserEnrolled;
            WorkshopBrowser.UserDisenrolled += ReservedBrowser.OnUserDisenrolled;
            WorkshopBrowser.WorkshopEvaluated += ReservedBrowser.OnWorkshopEvaluated;

            //When user does something on reservedBrowser, update workshopBrowser
            ReservedBrowser.UserDisenrolled += WorkshopBrowser.OnUserDisenrolled;
            ReservedBrowser.WorkshopEvaluated += WorkshopBrowser.OnWorkshopEvaluated;


            //When user does something from calendarView, update workshopBrowser and reservedBrowser
            CalendarPage.UserDisenrolled += ReservedBrowser.OnUserDisenrolled;
            CalendarPage.UserEnrolled += ReservedBrowser.OnUserEnrolled;
            CalendarPage.WorkshopEvaluated += ReservedBrowser.OnWorkshopEvaluated;

            CalendarPage.UserDisenrolled += WorkshopBrowser.OnUserDisenrolled;
            CalendarPage.UserEnrolled += WorkshopBrowser.OnUserEnrolled;
            CalendarPage.WorkshopEvaluated += WorkshopBrowser.OnWorkshopEvaluated;

            // Change number on badge of not evaluated workshops
            WorkshopBrowser.WorkshopEvaluated += OnWorkshopEvaluated;
            ReservedBrowser.WorkshopEvaluated += OnWorkshopEvaluated;
            CalendarPage.WorkshopEvaluated += OnWorkshopEvaluated;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!_onAppearingCalled)
            {
                var restResponse = await _restService.GetNonEvaluatedCount();
                if (restResponse.ResponseCode == HttpStatusCode.OK)
                {
                    Count = restResponse.Value;
                    if (Count == 0)
                    {
                        BindingContext = null;
                    }
                    else
                    {
                        BindingContext = Count;
                        var _currentApp = Application.Current as App;
                        _currentApp.UserEvaluated = Count;
                    }
                    _onAppearingCalled = true;
                }
            }
            

        }

        //Handler to reduce number of non evaluated workshop on badge
        public void OnWorkshopEvaluated(object sender, WorkshopDTO workshopDto)
        {            
            Count--;
            if (Count == 0)
            {
                BindingContext = null;
            }
            else
            {
                BindingContext = Count;

            }

        }

    }
}
