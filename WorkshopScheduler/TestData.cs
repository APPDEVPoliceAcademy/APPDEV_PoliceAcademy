using System;
using System.Collections.Generic;
using WorkshopScheduler.Models;

namespace WorkshopScheduler
{
    public class TestData
    {
        const string loremipsum =
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Donec quam felis, ultricies nec, pellentesque eu, pretium quis, sem. Nulla consequat massa quis enim.";

        private static List<Workshop> loremIpsumData = new List<Workshop>()
        {
            new Workshop()
            {
                Title = "PR for beginners",
                ShortDescription = "earliestTest",
                Description = loremipsum,
                Date = new DateTime(2014, 06, 13),
                Coach = "Andrzej Nowak",
                Place = "Windesheim"
            },

            new Workshop()
            {
                Title = "Team Project",
                ShortDescription = "latestTest",
                Description = loremipsum,
                Date = new DateTime(2019, 06, 21),
                Coach = "Tadeusz Sznuk",
                Place = "on-line"
            },
            new Workshop()
            {
                Title = "Team Building",
                ShortDescription = "firstAlphabeticalPlace",
                Description = loremipsum,
                Date = new DateTime(2018, 06, 21),
                Coach = "Andrzej Norek",
                Place = "Apeldorn"
            },

            new Workshop()
            {
                Title = "Leadership in practise",
                ShortDescription = "lastAlphabeticalPlace",
                Description = loremipsum,
                Date = new DateTime(2018, 06, 10),
                Coach = "Andrzej Nowak",
                Place = "Zwolle"
            },
            new Workshop()
            {
                Title = "Motivation",
                ShortDescription = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit",
                Description = loremipsum,
                Date = new DateTime(2018, 09, 21),
                Coach = "Andrzej Norek",
                Place = "on-line"
            },

            new Workshop()
            {
                Title = "aaTest",
                ShortDescription = "firstAlphabeticalTitle",
                Date = new DateTime(2017, 06, 18),
                Coach = "Andrzej Nowak",
                Place = "Groningen",
                Description = loremipsum
            },

            new Workshop()
            {
                Title = "xTest",
                ShortDescription = "lastAlphabeticalTitle",
                Date = new DateTime(2017, 06, 19),
                Description = loremipsum,
                Coach = "Andrzej Nowak",
                Place = "Windesheim"
            }
        };

        public static List<Workshop> LoremIpsumData
        {
            get => loremIpsumData;
            set => loremIpsumData = value;
        }
    }
}