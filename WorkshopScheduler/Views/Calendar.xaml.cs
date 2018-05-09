using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    public partial class Calendar : ContentPage
    {
        
        private int _currentYear = DateTime.Now.Year;
        private int _currentMonth = DateTime.Now.Month;
        private List<StackLayout> _gridStackLayouts = new List<StackLayout>(6 * 7);
        readonly RestService _restService = new RestService();
        CultureInfo _cultureInfo = new CultureInfo("nl");

        public Calendar()
        {
            InitializeComponent();
            InitializeCalendar();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await PopulateCalendar();
           
        }

        private void InitializeCalendar()
        {
            for (var i = 0; i < 6; i++)
            {
                var stackLayout = new StackLayout() {BackgroundColor = Color.DarkGray};
                stackLayout.Children.Add(new Label() {TextColor = Color.Black, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, Text = _cultureInfo.DateTimeFormat.AbbreviatedDayNames[i+1].ToUpper()});
                Grid.Children.Add(stackLayout, i, 0);
            }

            {
                var stackLayout = new StackLayout() { BackgroundColor = Color.DarkGray };
                stackLayout.Children.Add(new Label() { TextColor = Color.Black, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center, Text = _cultureInfo.DateTimeFormat.AbbreviatedDayNames[0].ToUpper() });
                Grid.Children.Add(stackLayout, 6, 0);
            }


            for (var j = 0; j < 6; j++)
            {
                for (var i = 0; i < 7; i++)
                {
                    var stackLayout = new StackLayout() { BackgroundColor = Color.White, Orientation = StackOrientation.Vertical, Padding = new Thickness(0, 10, 0, 5)};
                    stackLayout.Children.Add(new Label() {TextColor = Color.Gray, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center});
                    stackLayout.Children.Add(new Button() {BackgroundColor = Color.Black, WidthRequest = 6, HeightRequest = 6, CornerRadius = 3 , VerticalOptions = LayoutOptions.End, HorizontalOptions = LayoutOptions.Center});
                    _gridStackLayouts.Add(stackLayout);
                    Grid.Children.Add(stackLayout, i, j+1);
                }
            }
        }

        private async Task PopulateCalendar()
        {
            var takenDaysRepsonse = await _restService.GetDaysWithWorkshop(_currentMonth, _currentYear);
            List<int> takenDays = new List<int>();
            if (takenDaysRepsonse.ResponseCode == null)
            {
                await DisplayAlert("Error", takenDaysRepsonse.ErrorMessage + "\nMake sure that you have internet connection", "Ok");
            }

            if (takenDaysRepsonse.ResponseCode == HttpStatusCode.Unauthorized)
            {
                await DisplayAlert("Error", "Your session has expired. You will be redirected to log in", "Ok");
                Application.Current.MainPage = new LoginView();
            }


            if (takenDaysRepsonse.ResponseCode == HttpStatusCode.OK)
            {
                takenDays = takenDaysRepsonse.Value;

            }



            Month.Text = _cultureInfo.DateTimeFormat.GetMonthName(_currentMonth) + " " +
                         _currentYear.ToString();
            var daysNumber = DateTime.DaysInMonth(_currentYear, _currentMonth);
            var firstDayOfWeek = (new DateTime(_currentYear, _currentMonth, 1)).DayOfWeek;

            int firstDay;
            if (firstDayOfWeek == DayOfWeek.Sunday)
            {
                firstDay = 6;
            }
            else
            {
                firstDay = (int)firstDayOfWeek - 1;
            }

            var dayCounter = 1;

            for (var j = 0; j < 6; j++)
            {
                for (var i = 0; i < 7; i++)
                {
                    var label = _gridStackLayouts.ElementAt(j * 7 + i).Children[0] as Label;
                    var button = _gridStackLayouts.ElementAt(j * 7 + i).Children[1] as Button;
                    button.BackgroundColor = _gridStackLayouts.ElementAt(j * 7 + i).BackgroundColor;
                    _gridStackLayouts.ElementAt(j * 7 + i).GestureRecognizers.Clear();
                    if (dayCounter <= daysNumber && j * 7 + i >= firstDay)
                    {
                        if (takenDays.Contains(dayCounter))
                        {
                            var localCounter = dayCounter;
                            button.BackgroundColor = Color.Black;
                            var tap = new TapGestureRecognizer();
                            tap.Tapped += async (object sender, EventArgs e) =>
                            {
                                await DisplayAlert("Tap", "You tapped + " + localCounter, "Yes!");
                            };
                            _gridStackLayouts.ElementAt(j * 7 + i).GestureRecognizers.Add(tap);
                        }
                        label.Text = dayCounter.ToString();
                        dayCounter++;
                    }
                    else
                    {
                        label.Text = null;   
                    }
                }
            }
        }

        private async void ForwardButton_OnClicked(object sender, EventArgs e)
        {
            if (_currentMonth == 12)
            {
                _currentMonth = 1;
                _currentYear++;
            }
            else
            {
                _currentMonth++;
            }
            await PopulateCalendar();
        }

        private async void BackButton_OnClicked(object sender, EventArgs e)
        {
            if (_currentMonth == 1)
            {
                _currentMonth = 12;
                _currentYear--;
            }
            else
            {
                _currentMonth--;
            }
            await PopulateCalendar();
        }
    }
    }
