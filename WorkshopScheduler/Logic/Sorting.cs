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

        public  ObservableCollection<WorkshopDTO> ByDateAscending(ObservableCollection<WorkshopDTO> input)
        {
            return new ObservableCollection<WorkshopDTO>( input.OrderBy(x => x.Date).ToList());
           
        }

        public  ObservableCollection<WorkshopDTO> ByDateDescending(ObservableCollection<WorkshopDTO> input)
        {
            return new ObservableCollection<WorkshopDTO>(input.OrderByDescending(x => x.Date).ToList());
           
        }
    
        public  ObservableCollection<WorkshopDTO> ByTitleAscending(ObservableCollection<WorkshopDTO> input)
        {
            return new ObservableCollection<WorkshopDTO>(input.OrderBy(x => x.Title).ToList());
        }

        public  ObservableCollection<WorkshopDTO> ByTitleDescending(ObservableCollection<WorkshopDTO> input)
        {
            return new ObservableCollection<WorkshopDTO>(input.OrderByDescending(x => x.Title).ToList());
        }

        public  ObservableCollection<WorkshopDTO> ByPlaceAlphabeticalAscending(ObservableCollection<WorkshopDTO> input)
        {
            return new ObservableCollection<WorkshopDTO>(input.OrderBy(x => x.Place).ToList());
        }

        public  ObservableCollection<WorkshopDTO> ByPlaceAlphabeticalDescending(ObservableCollection<WorkshopDTO> input)
        {
            return new ObservableCollection<WorkshopDTO>(input.OrderByDescending(x => x.Place).ToList());
        }

    }
}
