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


        ObservableCollection<Workshop> FilterByDate(ObservableCollection<Workshop> input, DateTime startDate, DateTime endDate)
        {

        
            return new ObservableCollection<Workshop>(input.Where(a => (a.Date >= startDate && a.Date <= endDate)));
          
        }
        
        ObservableCollection<Workshop> FilterByPlace(ObservableCollection<Workshop> input, String desiredPlace)
        {
            return new ObservableCollection<Workshop>(input.Where(a => (a.Place == desiredPlace)));
          
        }
    }

 
}