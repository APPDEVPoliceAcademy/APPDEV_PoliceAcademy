using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace WorkshopScheduler.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FiltersModalView : ContentPage
    {

        List<string> source = new List<string> { "test1", "test2", "test3" };

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
