using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using WorkshopScheduler.Models;
using WorkshopScheduler.Logic;

namespace WorkshopScheduler.Views
{
    public partial class WorkshopBrowser : ContentPage
    {

        //public event EventHandler ChangeSorting;

       
        ObservableCollection<Workshop> workshopsList;
        ObservableCollection<Workshop> displayList;
        FiltersModalView filtersView;
        public WorkshopBrowser()
        {
            InitializeComponent();

            Sorting sortings = new Sorting();

            workshopsList = TestData.LoremIpsumData; // provide the test data
            WorkshopsListView.ItemsSource = workshopsList;

            filtersView = new FiltersModalView();

            filtersView.SortingChanged += (o, sortingChosen) =>
            {
                displayList = null;

                switch (sortingChosen)
                {
                    case SortingsEnum.ByDateAscending:
                        displayList = sortings.ByDateAscending(workshopsList);
                        break;
                    case SortingsEnum.ByDateDescending:
                        displayList = null;
                        break;
                    case SortingsEnum.ByTitleAscending:
                        displayList = sortings.ByTitleAscending(workshopsList);
                        break;
                    case SortingsEnum.ByTitleDescending:
                        displayList = sortings.ByTitleDescending(workshopsList);
                        break;
                    case SortingsEnum.None:
                        displayList = workshopsList;
                        break;
                    default:
                        DisplayAlert("couldn't match any", "shit", "ok");
                        break;
                }

                WorkshopsListView.ItemsSource = displayList;
              
            };

        }

        private void SearchWorkshop_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (SearchWorkshop.Text != null)
            {
                WorkshopsListView.ItemsSource = displayList.Where(x =>
                                                x.Title.IndexOf(SearchWorkshop.Text.Trim(' '), 0, StringComparison.CurrentCultureIgnoreCase) != -1).ToList();

            }
            else
            {
                WorkshopsListView.ItemsSource = workshopsList;
            }
        }

        async void SortingsButton_OnClicked(object sender, System.EventArgs e)
        {
               // DisplayAlert("refreshed",sortingChosen.ToString(),"ok");

            await Navigation.PushModalAsync(filtersView);

        }

    }
}
