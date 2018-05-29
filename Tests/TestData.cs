using System;
using System.Collections.Generic;
using WorkshopScheduler.Models;
using System.Collections.ObjectModel;


namespace WorkshopScheduler
{
    public class TestData
    {
        const string loremipsum =
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim.";

        private static ObservableCollection<WorkshopDTO> loremIpsumData = new ObservableCollection<WorkshopDTO>()
        {
            new WorkshopDTO()
            {
                Title = "PR for beginners",
                ShortDescription = "earliestTest",
                //Description = loremipsum,
                Date = new DateTime(2014, 06, 13),
                Coach = "Andrzej Nowak",
                Place = "Windesheim"
            },

            new WorkshopDTO()
            {
                Title = "Team Project",
                ShortDescription = "latestTest",
                //Description = loremipsum,
                Date = new DateTime(2019, 06, 21),
                Coach = "Tadeusz Sznuk",
                Place = "on-line"
            },
            new WorkshopDTO()
            {
                Title = "Team Building",
                ShortDescription = "firstAlphabeticalPlace",
                //Description = loremipsum,
                Date = new DateTime(2018, 06, 21),
                Coach = "Andrzej Norek",
                Place = "Apeldorn"
            },

            new WorkshopDTO()
            {
                Title = "Leadership in practise",
                ShortDescription = "lastAlphabeticalPlace",
                //Description = loremipsum,
                Date = new DateTime(2018, 06, 10),
                Coach = "Andrzej Nowak",
                Place = "Zwolle"
            },
            new WorkshopDTO()
            {
                Title = "Motivation",
                ShortDescription = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit",
              //  Description = loremipsum,
                Date = new DateTime(2018, 09, 21),
                Coach = "Andrzej Norek",
                Place = "on-line"
            },

            new WorkshopDTO()
            {
                Title = "aaTest",
                ShortDescription = "firstAlphabeticalTitle",
                Date = new DateTime(2017, 06, 18),
                Coach = "Andrzej Nowak",
                Place = "Groningen",
            //    Description = loremipsum
            },

            new WorkshopDTO()
            {
                Title = "xTest",
                ShortDescription = "lastAlphabeticalTitle",
                Date = new DateTime(2017, 06, 19),
                //Description = loremipsum,
                Coach = "Andrzej Nowak",
                Place = "Windesheim"
            }
        };

        public static ObservableCollection<WorkshopDTO> LoremIpsumData
        {
            get => loremIpsumData;
            set => loremIpsumData = value;
        }
    }
}