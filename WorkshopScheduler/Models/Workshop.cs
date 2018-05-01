using System;
namespace WorkshopScheduler.Models
{
    public class Workshop
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public DateTime Date { get; set; }
        public string Coach { get; set; }
        public string Place { get; set; }
        public bool IsEnrolled { get; set; }
        public bool IsNotEnrolled => !IsEnrolled;
        public bool IsEvaluated { get; set; }
        public string EvaluationUri { get; set; }
        public int NumberOfSpots { get; set; }
        public int TakenSpots { get; set; } 
        public bool IsWithin12Weeks => Date.CompareTo(DateTime.Now.AddDays(12 * 7)) < 0;
        public string Spots => TakenSpots + "/" + NumberOfSpots;

        public override bool Equals(object obj)
        {
            var workshopObj = obj as WorkshopDTO; 
            if (workshopObj == null)
                return false;
            else
            {

                return (this.Title.Equals(workshopObj.Title) &&
                               this.Date.Equals(workshopObj.Date) &&
                               this.Coach.Equals(workshopObj.Coach) &&
                               //this.Description.Equals(workshopObj.Description) &&
                               this.Place.Equals(workshopObj.Place) &&
                               this.ShortDescription.Equals(workshopObj.ShortDescription));
                
            }
        }
    }

}
