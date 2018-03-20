using System;
using System.Collections.Generic;
using NUnit.Framework;
using WorkshopScheduler;
using WorkshopScheduler.Logic;
using WorkshopScheduler.Models;


namespace Tests
{
    [TestFixture]
    public class SortingTest
    {
        private List<Workshop> _input;

        Workshop firstByDate = TestData.LoremIpsumData.Find(workshop => workshop.ShortDescription.Equals("earliestTest"));
        Workshop lastByDate = TestData.LoremIpsumData.Find(workshop => workshop.ShortDescription.Equals("latestTest"));
        Workshop firstByTitle = TestData.LoremIpsumData.Find(workshop => workshop.ShortDescription.Equals("firstAlphabeticalTitle"));
        Workshop lastByTitle = TestData.LoremIpsumData.Find(workshop => workshop.ShortDescription.Equals("lastAlphabeticalTitle"));
        Workshop firstByAlphabeticalPlace = TestData.LoremIpsumData.Find(workshop => workshop.ShortDescription.Equals("firstAlphabeticalPlace"));
        Workshop lastByAlphabeticalPlace = TestData.LoremIpsumData.Find(workshop => workshop.ShortDescription.Equals("lastAlphabeticalPlace"));

        
        [SetUp]
        public void SetUp()
        {
            _input = TestData.LoremIpsumData;
        }

        [Test]
        public void ByDateAscendingTest()
        {
            var sorted = Sorting.ByDateAscending(_input);
            Assert.AreEqual(firstByDate, sorted[0]);
            Assert.AreEqual(lastByDate, sorted[sorted.Count - 1]);
        }

        [Test]
        public void ByDateDescendingTest()
        {
            var sorted = Sorting.ByDateDescending(_input);
            Assert.AreEqual(lastByDate, sorted[0]);
            Assert.AreEqual(firstByDate, sorted[sorted.Count - 1]);
        }

        [Test]
        public void ByTitleAscendingTest()
        {
            var sorted = Sorting.ByTitleAscending(_input);
            Assert.AreEqual(firstByTitle, sorted[0]);
            Assert.AreEqual(lastByTitle, sorted[sorted.Count - 1]);
        }

        [Test]
        public void ByTitleDescendingTest()
        {
            var sorted = Sorting.ByTitleDescending(_input);
            Assert.AreEqual(lastByTitle, sorted[0]);
            Assert.AreEqual(firstByTitle, sorted[sorted.Count - 1]);
        } 
        
        [Test]
        public void ByPlaceAscendingTest()
        {
            var sorted = Sorting.ByPlaceAlphabeticalAscending(_input);
            Console.WriteLine(firstByAlphabeticalPlace.Title);
            Console.WriteLine(sorted[0].Title);
            Console.WriteLine(lastByAlphabeticalPlace.Title);
            Console.WriteLine(sorted[sorted.Count-1].Title);
            Assert.AreEqual(firstByAlphabeticalPlace, sorted[0]);
            Assert.AreEqual(lastByAlphabeticalPlace, sorted[sorted.Count - 1]);
        }

        [Test]
        public void ByPlaceDescendingTest()
        {
            var sorted = Sorting.ByPlaceAlphabeticalDescending(_input);
            Assert.AreEqual(lastByAlphabeticalPlace, sorted[0]);
            Assert.AreEqual(firstByAlphabeticalPlace, sorted[sorted.Count - 1]);
        } 
    }
}