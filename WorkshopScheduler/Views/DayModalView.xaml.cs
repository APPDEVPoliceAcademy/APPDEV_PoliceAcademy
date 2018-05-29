using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using WorkshopScheduler.Models;
using WorkshopScheduler.RestLogic;
using WorkshopScheduler.Views.UserAccountViews;
using Xamarin.Forms;

namespace WorkshopScheduler.Views
{
	public partial class DayModalView : ContentPage
	{
		ObservableCollection<WorkshopDTO> _workshopsList;
		public event EventHandler<WorkshopDTO> UserEnrolled;
		public event EventHandler<WorkshopDTO> UserDisenrolled;
		public event EventHandler<WorkshopDTO> WorkshopEvaluated;
        private IRestService _restService = new RestService();
	    private int _month;
	    private int _day;
	    private int _year;
	    private readonly CultureInfo _cultureInfo = new CultureInfo("nl");


        public DayModalView(int year, int month, int day)
		{
			InitializeComponent();
		    _month = month;
		    _year = year;
		    _day = day;
		    DateLabel.Text = new DateTime(year, month, day).ToString("D", _cultureInfo);


            _workshopsList = new ObservableCollection<WorkshopDTO>();

		    
		
		}

		async void BackButton_OnClick(object sender, System.EventArgs e)
		{
			await Navigation.PopModalAsync();
		}
       

		protected override async void OnAppearing()
		{
			base.OnAppearing();


		    var restRespone = await _restService.GetWorkshopsForDay(_year, _month, _day);

		    if (restRespone.ResponseCode == null)
		    {
		        await DisplayAlert("Error", restRespone.ErrorMessage + "\nMake sure that you have internet connection", "Ok");
		    }

		    if (restRespone.ResponseCode == HttpStatusCode.Unauthorized)
		    {
		        //Check token validation additionaly
		        await DisplayAlert("Error", "Your session has expired. You will be redirected to log in", "Ok");
		        WorkshopsListView.ItemsSource = new ObservableCollection<WorkshopDTO>();
		        Application.Current.MainPage = new LoginView();
		    }

		    if (restRespone.ResponseCode == HttpStatusCode.OK)
		    {
		        _workshopsList = new ObservableCollection<WorkshopDTO>(restRespone.Value);
		    }

            WorkshopsListView.ItemsSource = _workshopsList;
	    }

		private async void WorkshopsListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null) return;
			var currentItem = e.SelectedItem as WorkshopDTO;
			var workshopDetailpage = new WorkshopDetail(currentItem.Id);
			workshopDetailpage.UserEnrolled += (o, workshop) =>
			{
				var workshopDto = _workshopsList.FirstOrDefault(dto => dto.Id == workshop.Id);
				if (workshopDto != null)
				{
					workshopDto.IsEnrolled = true;
					workshopDto.TakenSpots++;
				}
				UserEnrolled?.Invoke(this, workshopDto);
			};
			workshopDetailpage.UserDisenrolled += (o, workshop) =>
			{
				var workshopDto = _workshopsList.FirstOrDefault(dto => dto.Id == workshop.Id);
				if (workshopDto != null)
				{
					workshopDto.IsEnrolled = false;
					workshopDto.TakenSpots--;
				}
				UserDisenrolled?.Invoke(this, workshopDto);
			};
			workshopDetailpage.WorkshopEvaluated += (o, workshop) =>
			{
				var workshopDto = _workshopsList.FirstOrDefault(dto => dto.Id == workshop.Id);
				if (workshopDto != null) workshopDto.IsEvaluated = true;
				WorkshopEvaluated?.Invoke(this, workshopDto);

			};


			await Navigation.PushModalAsync(workshopDetailpage);
			WorkshopsListView.SelectedItem = null;
		}
	}
}
