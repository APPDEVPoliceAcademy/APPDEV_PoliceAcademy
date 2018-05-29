using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using WorkshopScheduler.Logic;

namespace WorkshopScheduler.Views
{

    public partial class FiltersModalView : ContentPage
    {
     

        public event EventHandler<SortingsEnum> SortingChanged;
        public event EventHandler<bool> WeeksFilterChanged;
        public event EventHandler<DatesSortingEventArgs> DatesFilterChanged;
        public event EventHandler<Unit> UnitFilterChanged;
        public event EventHandler<bool> IsFinished;


        List<SortingsEnum> sourceSortings = Enum.GetValues(typeof(SortingsEnum)).Cast<SortingsEnum>().ToList();
        List<Unit> placesSortings = Enum.GetValues(typeof(Unit)).Cast<Unit>().ToList();

        private WorkshopBrowserType _type;

        public FiltersModalView(WorkshopBrowserType type)
        {
            InitializeComponent();
            _type = type;
            if (type == WorkshopBrowserType.All)
            {
                startDate.Date = DateTime.Now;
                endDate.Date = DateTime.Now.AddYears(2);
            }
            else
            {
                startDate.Date = DateTime.Now.AddYears(-5);
                endDate.Date = DateTime.Now.AddYears(2);
            }
            
            datePickerSwitch.IsToggled = true;
            

            //populate pickets
            sortingPicker.ItemsSource = sourceSortings;
            PlacesPicker.ItemsSource = placesSortings;

            sortingPicker.SelectedIndex = sourceSortings.FindIndex(en => en == SortingsEnum.ByDateAscending);
        }

        async void BackButton_OnClicked(object sender, System.EventArgs e)
        {
            IsFinished?.Invoke(this, true);
            await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            IsFinished?.Invoke(this,true);
            return base.OnBackButtonPressed();
        }


        void SortingPicker_OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "SelectedItem") return;
            var choice = (SortingsEnum)Enum.Parse(typeof(SortingsEnum), sortingPicker.Items[sortingPicker.SelectedIndex]);
            SortingChanged?.Invoke(this, choice);
        }

        void UnitPicker_OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == "SelectedItem")
            {
                //string choice = sortingPicker.Items[sortingPicker.SelectedIndex];

                var choice = (SortingsEnum)Enum.Parse(typeof(SortingsEnum), sortingPicker.Items[sortingPicker.SelectedIndex]);
                SortingChanged?.Invoke(this, choice);
            }
        }

        void OnWeeksSwitchToggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            WeeksFilterChanged?.Invoke(this, weeksSwitch.IsToggled);
        }

        void OnDateSwitchToggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            DateTime[] datesEventArgs = {startDate.Date, endDate.Date };
            var args = new DatesSortingEventArgs {dates = datesEventArgs};
            args.isOn = e == null || e.Value;
            DatesFilterChanged?.Invoke(this, args);
        }

        async void OnResetButtonClicked(object sender, System.EventArgs e)
        {
            if (_type == WorkshopBrowserType.All)
            {
                startDate.Date = DateTime.Now;
                endDate.Date = DateTime.Now.AddYears(2);
            }
            else
            {
                startDate.Date = DateTime.Now.AddYears(-5);
                endDate.Date = DateTime.Now.AddYears(2);
            }
            datePickerSwitch.IsToggled = true;
            weeksSwitch.IsToggled = false;

            sortingPicker.SelectedIndex = sourceSortings.FindIndex(en => en == SortingsEnum.ByDateAscending);

            IsFinished?.Invoke(this, true);
            await Navigation.PopModalAsync();
        }

        void DatePicker_OnChange(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (datePickerSwitch.IsToggled)
                OnDateSwitchToggled(this, null);
        }
    }
}
