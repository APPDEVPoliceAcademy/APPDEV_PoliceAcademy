using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WorkshopScheduler.Models;
using WorkshopScheduler.RestLogic;
using WorkshopScheduler.Views.UserAccountViews;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkshopScheduler.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkshopDetail : ContentPage
    {
        public event EventHandler<Workshop> UserEnrolled;
        public event EventHandler<Workshop> UserDisenrolled;
        public event EventHandler<Workshop> WorkshopEvaluated;
        private static  List<Workshop> cachedWorkshops = new List<Workshop>();
        private IRestService _restService = new RestService();
        private int _currentWorkshopID;
        private Workshop _currentWorkshop;

        public WorkshopDetail(int currentWorkshopID)
        {
            _currentWorkshopID = currentWorkshopID;
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //if workshop is already cached

//            var cachedWorkshop = cachedWorkshops?.FirstOrDefault(workshop => workshop.Id == _currentWorkshopID);
//           if (cachedWorkshop != null)
//            {
//                _currentWorkshop = cachedWorkshop;
//                LinksListView.ItemsSource = _currentWorkshop.Files;
//                HideIndicator();
//            }

//            else
//            {

                var workshopsResponse = await _restService.GetSingleWorkshop(_currentWorkshopID);

                if (workshopsResponse.ResponseCode == null)
                {
                    await DisplayAlert("Error",
                        workshopsResponse.ErrorMessage + "\nMake sure that you have internet connection", "Ok");
                    Navigation.PopAsync();
                }

                if (workshopsResponse.ResponseCode == HttpStatusCode.Unauthorized)
                {
                    //Check token validation additionaly
                    await DisplayAlert("Error", "Your session has expired. You will be redirected to log in", "Ok");
                    Application.Current.MainPage = new LoginView();
                }

                if (workshopsResponse.ResponseCode == HttpStatusCode.OK)
                {
                    _currentWorkshop = workshopsResponse.Value;
                    LinksListView.ItemsSource = _currentWorkshop.Files;
                    cachedWorkshops?.Add(_currentWorkshop);
                    HideIndicator();
                }
                
//            }
            BindingContext = _currentWorkshop;

            setButtons();

        }


        private void BackButton_OnClicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void ApplyButton_OnClicked(object sender, EventArgs e)
        {
            if (_currentWorkshop.IsWithin12Weeks)
            {
                await DisplayAlert("Alert", "You are about to sign for workshop within 12 weeks\n" +
                                      "Do you want to send email to your scheduler?",
                    "Yes take me to mail app",
                    "Continue anyway");
            }
            else
            {
                await DisplayAlert("Sign in", "Signing in for workshop", "Ok");
            }

            var restResponse = await _restService.EnrollUser(_currentWorkshopID);

            if (restResponse.ResponseCode == null)
            {
                await DisplayAlert("Error",
                    restResponse.ErrorMessage + "\nMake sure that you have internet connection", "Ok");
                return;
            }

            if (restResponse.ResponseCode == HttpStatusCode.Unauthorized)
            {
                //Check token validation additionaly
                await DisplayAlert("Error", "Your session has expired. You will be redirected to log in", "Ok");
                Application.Current.MainPage = new LoginView();
            }

            if (restResponse.ResponseCode == HttpStatusCode.OK)
            {
                UserEnrolled?.Invoke(this, _currentWorkshop);
                _currentWorkshop.IsEnrolled = true;
                _currentWorkshop.TakenSpots++;
                await Navigation.PopModalAsync();
            }
        }

        private async void DisenrollButton_OnClicked(object sender, EventArgs e)
        {
            var restResponse = await _restService.DisenrollUser(_currentWorkshopID);

            if (restResponse.ResponseCode == null)
            {
                await DisplayAlert("Error",
                    restResponse.ErrorMessage + "\nMake sure that you have internet connection", "Ok");
                return;
            }

            if (restResponse.ResponseCode == HttpStatusCode.Unauthorized)
            {
                //Check token validation additionaly
                await DisplayAlert("Error", "Your session has expired. You will be redirected to log in", "Ok");
                Application.Current.MainPage = new LoginView();
            }

            if (restResponse.ResponseCode == HttpStatusCode.OK)
            {
                UserDisenrolled?.Invoke(this, _currentWorkshop);
                _currentWorkshop.IsEnrolled = false;
                _currentWorkshop.TakenSpots--;
                await Navigation.PopModalAsync();
            }
        }

        private async void CloseWebView_OnClicked(object sender, EventArgs e)
        {
            var decision = await DisplayAlert("Warning", "Have you fulfiled evaluation form?", "Yes", "No");
            if (decision)
            {
                var restResponse = await _restService.EvaluateWorkshop(_currentWorkshopID);
                if (restResponse.ResponseCode == null)
                {
                    await DisplayAlert("Error",
                        restResponse.ErrorMessage + "\nMake sure that you have internet connection", "Ok");
                    return;
                }

                if (restResponse.ResponseCode == HttpStatusCode.Unauthorized)
                {
                    //Check token validation additionaly
                    await DisplayAlert("Error", "Your session has expired. You will be redirected to log in", "Ok");
                    Application.Current.MainPage = new LoginView();
                }

                if (restResponse.ResponseCode == HttpStatusCode.OK)
                {
                    WorkshopEvaluated?.Invoke(this, _currentWorkshop);
                    _currentWorkshop.IsEvaluated = true;
                    EvaluateButton.IsVisible = false;
                }
            }

            WebStackLayout.IsVisible = false;
        }

        private void EvaluateButton_OnClicked(object sender, EventArgs e)
        {



            WebViewLayout.Children.Add(new WebView()
                {
                    Source = _currentWorkshop.EvaluationUri,
                    HeightRequest = 1000,
                    WidthRequest = 800
                },
                widthConstraint: Constraint.RelativeToParent(layout => layout.Width),
                heightConstraint: Constraint.RelativeToParent(layout => layout.Height * 0.9));
            WebStackLayout.IsVisible = true;
        }


        private void HideIndicator()
        {
            ActivityIndicator.IsRunning = false;
            ActivityIndicator.IsVisible = false;
            GridView.IsVisible = true;
        }

        private void setButtons()
        {
            // This is past workshop
            if (_currentWorkshop.Date < DateTime.Now)
            {
                ApplyButton.IsVisible = false;
                DisenrollButton.IsVisible = false;
                //User took part and did not evaluate workshop
                if (_currentWorkshop.IsEnrolled && !_currentWorkshop.IsEvaluated)
                {
                    EvaluateButton.IsVisible = true;
                }
                //User eitheir did not take part or did evaluate it already 
                else
                {
                    EvaluateButton.IsVisible = false;
                }
                
            }
            //It's future workshop
            else
            {
                EvaluateButton.IsVisible = false;
                //User is enrolled, show option to disenroll
                if (_currentWorkshop.IsEnrolled)
                {
                    ApplyButton.IsVisible = false;
                    DisenrollButton.IsVisible = true;
                }
                //User is not enrolled, show option to enroll
                else
                {
                    ApplyButton.IsVisible = true;
                    DisenrollButton.IsVisible = false;
                }
            }
        }


        private async void LinksListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            var uri = e.SelectedItem as Link;
            var names = uri.Uri.Split('\\');

            await _restService.SaveFile(uri.Uri, names.Last());
            LinksListView.SelectedItem = null;

        }
    }
}