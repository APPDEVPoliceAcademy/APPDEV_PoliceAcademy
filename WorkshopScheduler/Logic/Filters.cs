using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using WorkshopScheduler.Models;

namespace WorkshopScheduler.Logic
{
    
    public class Filters
    {
        List<Workshop> FilterByDate(List<Workshop> input, DateTime startDate, DateTime endDate)
        {
            var filtered = input.FindAll(a => (a.Date >= startDate && a.Date <= endDate));
            return null;
        }
        
        List<Workshop> FilterByTitle(List<Workshop> input, String desiredTitle)
        {
            var filtered = input.FindAll(a => (a.Title.Contains(desiredTitle)));
            return null;
        }
        
        List<Workshop> FilterByPlace(List<Workshop> input, String desiredPlace)
        {
            var filtered = input.FindAll(a => (a.Place.Contains(desiredPlace)));
            return null;
        }
    }
}