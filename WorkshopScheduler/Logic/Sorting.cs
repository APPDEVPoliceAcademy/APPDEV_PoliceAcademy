using System;
using System.Linq;
using System.Collections.Generic;
using WorkshopScheduler.Models;

namespace WorkshopScheduler.Logic
{
    public static class Sorting
    {
        //TODO: check if it shouldn't be on the oposite way XD
        public static List<Workshop> ByDateAscending(List<Workshop> input)
        {
            input.Sort((a, b) => (a.Date.CompareTo(b.Date)));
            return input;
        }

        public static List<Workshop> ByDateDescending(List<Workshop> input)
        {
            input.Sort((a, b) => (a.Date.CompareTo(b.Date)*(-1)));
            return input;
        }
    
        public static List<Workshop> ByTitleAscending(List<Workshop> input)
        {
            input.Sort((a, b) => (string.Compare(a.Title, b.Title, StringComparison.CurrentCulture)));
            return input;
        }

        public static List<Workshop> ByTitleDescending(List<Workshop> input)
        {
            input.Sort((a, b) => (string.Compare(a.Title, b.Title, StringComparison.CurrentCulture)*(-1)));
            return input;
        }

        public static List<Workshop> ByPlaceAlphabeticalAscending(List<Workshop> input)
        {
            input.Sort((a, b) => (string.Compare(a.Place, b.Place, StringComparison.CurrentCulture)));
            return input;
        }

        public static List<Workshop> ByPlaceAlphabeticalDescending(List<Workshop> input)
        {
            input.Sort((a, b) => (string.Compare(a.Place, b.Place, StringComparison.CurrentCulture) * (-1)));
            return input;
        }

    }
}
