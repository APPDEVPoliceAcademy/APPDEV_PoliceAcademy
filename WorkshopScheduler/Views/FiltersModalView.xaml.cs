using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
namespace WorkshopScheduler.Views
{

    public partial class FiltersModalView : ContentPage
    {
     

        public event EventHandler<SortingsEnum> SortingChanged;
        public event EventHandler<bool> WeeksFilterChanged;
        public event EventHandler<DateTime[]> DatesFilterChanged;
        public event EventHandler<Unit> UnitFilterChanged;
        public event EventHandler ResetSettings;

        List<SortingsEnum> sourceSortings = Enum.GetValues(typeof(SortingsEnum)).Cast<SortingsEnum>().ToList();
        List<Unit> placesSortings = Enum.GetValues(typeof(Unit)).Cast<Unit>().ToList();

        public FiltersModalView()
        {
            InitializeComponent();

            //populate pickets
            sortingPicker.ItemsSource = sourceSortings;
            PlacesPicker.ItemsSource = placesSortings;
        }

        async void BackButton_OnClicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }


        void SortingPicker_OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedItem")
            {
                //string choice = sortingPicker.Items[sortingPicker.SelectedIndex];

                var choice = (SortingsEnum)Enum.Parse(typeof(SortingsEnum), sortingPicker.Items[sortingPicker.SelectedIndex]);
                SortingChanged.Invoke(this, choice);
            }
        }

        void UnitPicker_OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == "SelectedItem")
            {
                //string choice = sortingPicker.Items[sortingPicker.SelectedIndex];

                var choice = (SortingsEnum)Enum.Parse(typeof(SortingsEnum), sortingPicker.Items[sortingPicker.SelectedIndex]);
                SortingChanged.Invoke(this, choice);
            }
        }

        void OnWeeksSwitchToggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            WeeksFilterChanged(this, weeksSwitch.IsToggled);
        }

        void OnDateSwitchToggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            DateTime[] dates = {startDate.Date, endDate.Date };
            DatesFilterChanged(this, dates);
        }

        async void OnResetButtonClicked(object sender, System.EventArgs e)
        {
            ResetSettings(this, e);
            await Navigation.PopModalAsync();
        }

        void DatePicker_OnChange(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (datePickerSwitch.IsToggled)
                OnDateSwitchToggled(this, null);
        }
    }
}
