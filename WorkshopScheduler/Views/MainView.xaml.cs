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



            WorkshopBrowser.UserEnrolled += ReservedBrowser.OnUserEnrolled;
            WorkshopBrowser.UserDisenrolled += ReservedBrowser.OnUserDisenrolled;
            WorkshopBrowser.WorkshopEvaluated += ReservedBrowser.OnWorkshopEvaluated;

            ReservedBrowser.UserDisenrolled += WorkshopBrowser.OnUserDisenrolled;
            ReservedBrowser.WorkshopEvaluated += WorkshopBrowser.OnWorkshopEvaluated;

            WorkshopBrowser.WorkshopEvaluated += OnWorkshopEvaluated;
            ReservedBrowser.WorkshopEvaluated += OnWorkshopEvaluated;
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
