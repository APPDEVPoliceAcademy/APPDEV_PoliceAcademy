using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NUnit.Framework;
using WorkshopScheduler;
using WorkshopScheduler.Logic;
using WorkshopScheduler.Models;


namespace Tests
{
    [TestFixture]
    public class SortingTest
    {
        private ObservableCollection<WorkshopDTO> _input;
        private Sorting _sorting;

        WorkshopDTO firstByDate = new List<WorkshopDTO>(TestData.LoremIpsumData).Find(workshop => workshop.ShortDescription.Equals("earliestTest"));
        WorkshopDTO lastByDate = new List<WorkshopDTO>(TestData.LoremIpsumData).Find(workshop => workshop.ShortDescription.Equals("latestTest"));
        WorkshopDTO firstByTitle = new List<WorkshopDTO>(TestData.LoremIpsumData).Find(workshop => workshop.ShortDescription.Equals("firstAlphabeticalTitle"));
        WorkshopDTO lastByTitle = new List<WorkshopDTO>(TestData.LoremIpsumData).Find(workshop => workshop.ShortDescription.Equals("lastAlphabeticalTitle"));
        WorkshopDTO firstByAlphabeticalPlace = new List<WorkshopDTO>(TestData.LoremIpsumData).Find(workshop => workshop.ShortDescription.Equals("firstAlphabeticalPlace"));
        WorkshopDTO lastByAlphabeticalPlace = new List<WorkshopDTO>(TestData.LoremIpsumData).Find(workshop => workshop.ShortDescription.Equals("lastAlphabeticalPlace"));

        
        [SetUp]
        public void SetUp()
        {
            _input = TestData.LoremIpsumData;
            _sorting = new Sorting();
        }

        [Test]
        public void ByDateAscendingTest()
        {
            var sorted = _sorting.ByDateAscending(_input);
            Assert.AreEqual(firstByDate, sorted[0]);
            Assert.AreEqual(lastByDate, sorted[sorted.Count - 1]);
        }

        [Test]
        public void ByDateDescendingTest()
        {
            var sorted = _sorting.ByDateDescending(_input);
            Assert.AreEqual(lastByDate, sorted[0]);
            Assert.AreEqual(firstByDate, sorted[sorted.Count - 1]);
        }

        [Test]
        public void ByTitleAscendingTest()
        {
            var sorted = _sorting.ByTitleAscending(_input);
            Assert.AreEqual(firstByTitle, sorted[0]);
            Assert.AreEqual(lastByTitle, sorted[sorted.Count - 1]);
        }

        [Test]
        public void ByTitleDescendingTest()
        {
            var sorted = _sorting.ByTitleDescending(_input);
            Assert.AreEqual(lastByTitle, sorted[0]);
            Assert.AreEqual(firstByTitle, sorted[sorted.Count - 1]);
        } 
        
        [Test]
        public void ByPlaceAscendingTest()
        {
            var sorted = _sorting.ByPlaceAlphabeticalAscending(_input);
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
            var sorted = _sorting.ByPlaceAlphabeticalDescending(_input);
            Assert.AreEqual(lastByAlphabeticalPlace, sorted[0]);
            Assert.AreEqual(firstByAlphabeticalPlace, sorted[sorted.Count - 1]);
        } 
    }
}