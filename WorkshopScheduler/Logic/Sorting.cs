using System;
using System.Linq;
using System.Collections.Generic;
using WorkshopScheduler.Models;
using WorkshopScheduler.Views;
using System.Collections.ObjectModel;

namespace WorkshopScheduler.Logic
{
    public  class Sorting
    {

        public  ObservableCollection<Workshop> ByDateAscending(ObservableCollection<Workshop> input)
        {
            return new ObservableCollection<Workshop>( input.OrderBy(x => x.Date).ToList());
           
        }

        public  ObservableCollection<Workshop> ByDateDescending(ObservableCollection<Workshop> input)
        {
            return new ObservableCollection<Workshop>(input.OrderByDescending(x => x.Date).ToList());
           
        }
    
        public  ObservableCollection<Workshop> ByTitleAscending(ObservableCollection<Workshop> input)
        {
            return new ObservableCollection<Workshop>(input.OrderBy(x => x.Title).ToList());
        }

        public  ObservableCollection<Workshop> ByTitleDescending(ObservableCollection<Workshop> input)
        {
            return new ObservableCollection<Workshop>(input.OrderByDescending(x => x.Title).ToList());
        }

        public  ObservableCollection<Workshop> ByPlaceAlphabeticalAscending(ObservableCollection<Workshop> input)
        {
            return new ObservableCollection<Workshop>(input.OrderBy(x => x.Place).ToList());
        }

        public  ObservableCollection<Workshop> ByPlaceAlphabeticalDescending(ObservableCollection<Workshop> input)
        {
            return new ObservableCollection<Workshop>(input.OrderByDescending(x => x.Place).ToList());
        }

    }
}
