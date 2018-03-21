using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace WorkshopScheduler.Views
{
    enum Sortings { None, ByDateAscending, ByDateDescending, ByTitleAscending, ByTitleDescending };

    public partial class FiltersModalView : ContentPage
    {

        List<string> source = Enum.GetNames(typeof(Sortings)).ToList();

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
                testlabel.Text = sortingPicker.Items[sortingPicker.SelectedIndex];


            } 
        }

    }
}
