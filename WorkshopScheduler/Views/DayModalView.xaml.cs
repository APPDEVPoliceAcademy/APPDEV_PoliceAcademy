using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WorkshopScheduler.Models;
using Xamarin.Forms;

namespace WorkshopScheduler.Views
{
	public partial class DayModalView : ContentPage
	{
		ObservableCollection<WorkshopDTO> workshopsList;
		public event EventHandler<WorkshopDTO> UserEnrolled;
		public event EventHandler<WorkshopDTO> UserDisenrolled;
		public event EventHandler<WorkshopDTO> WorkshopEvaluated;

	

		public DayModalView()
		{
			InitializeComponent();
			DateLabel.Text = DateTime.Now.ToString("D");

			workshopsList = new ObservableCollection<WorkshopDTO>();

		    
		
		}

		async void BackButton_OnClick(object sender, System.EventArgs e)
		{
			await Navigation.PopModalAsync();
		}
       

		protected override void OnAppearing()
		{
			base.OnAppearing();

            
			//	DisplayAlert("titlexd", "Hi! \n I am empty", " :<");

			//workshopsList.Add(new WorkshopDTO()
			//{
			//	Id = 1,
			//	Title = "PR for dummies",
			//	Coach = "Grzegorz Brzęczyszczykiewicz",
			//	ShortDescription = null,
			//	Date = DateTime.Now,
			//	Place = "Apeldoorn",
			//	NumberOfSpots = 30,
			//	TakenSpots = 1,
			//	IsEnrolled = false,
			//	IsEvaluated = false
			//});

            

                WorkshopsListView.ItemsSource = workshopsList;
				//DisplayAlert("titlexd", "Not anymore", " :>");
			}

        public void PopulateModal(int id)
		{
			DisplayAlert("test", id.ToString(), "test");
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
	}
}
