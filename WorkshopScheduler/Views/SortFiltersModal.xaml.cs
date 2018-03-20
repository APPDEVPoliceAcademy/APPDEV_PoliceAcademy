using System.Collections.Generic;
using Xamarin.Forms;

namespace WorkshopScheduler.Views
{
    public partial class SortFiltersModal : ContentPage
    {
        Color active = Color.Accent;
        Color unactive = Color.Gray;
        
        public SortFiltersModal()
        {
            InitializeComponent();
            sortingPicker.ItemsSource = new List<string> {"test1", "test2", "test3"};
        }

        void Handle_BackButton(object sender, System.EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        void Handle_ChangeSorting(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            switch (button.ClassId)
            {
                case "TitleAlphabetical":
                    button.TextColor = active;
                    break;
                case "TitleReverseAlphabetical":

                    break;
                case "PlaceAlphabetical":
                    
                    break;
                case "PlaceReverseAlphabetical":
                    
                    break;
            }
        }
    }
}