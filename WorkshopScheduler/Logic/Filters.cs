using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using WorkshopScheduler.Models;
using System.Collections.ObjectModel;

namespace WorkshopScheduler.Logic
{

    public class Filters
    {


        public ObservableCollection<WorkshopDTO> FilterByDate(ObservableCollection<WorkshopDTO> input, DateTime[] dates)
        {
            return new ObservableCollection<WorkshopDTO>(input.Where(a => (a.Date >= dates[0].Date && a.Date <= dates[1].Date)));
        }

        public ObservableCollection<WorkshopDTO> FilterByPlace(ObservableCollection<WorkshopDTO> input, String desiredPlace)
        {
            return new ObservableCollection<WorkshopDTO>(input.Where(a => (a.Place == desiredPlace)));
        }

        public ObservableCollection<WorkshopDTO> FilterBy12Weeks(ObservableCollection<WorkshopDTO> input, bool flag)
        {
            if (flag)
                return new ObservableCollection<WorkshopDTO>(input.Where(a => (a.Date <= DateTime.Now.AddDays(84)) && a.Date >= DateTime.Now));
            
                return input;
        }
    }


}