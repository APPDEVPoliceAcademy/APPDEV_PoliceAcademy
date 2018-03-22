using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
namespace WorkshopScheduler.Views
{
  
    public partial class FiltersModalView : ContentPage 
    {

        public event EventHandler<SortingsEnum> SortingChanged;


        List<SortingsEnum> source = Enum.GetValues(typeof(SortingsEnum)).Cast<SortingsEnum>().ToList();

        public FiltersModalView()
        {
            InitializeComponent();


            sortingPicker.ItemsSource = source;
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

                var choice =(SortingsEnum)Enum.Parse(typeof(SortingsEnum), sortingPicker.Items[sortingPicker.SelectedIndex]);
                SortingChanged.Invoke(this, choice);
            } 
        }

    }
}
