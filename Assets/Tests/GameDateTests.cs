using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Employees;
using GameTime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using DayOfWeek = GameTime.DayOfWeek;

namespace Assets.Tests
{
    /// <summary>
    /// Tests for GameDate.
    /// </summary>
    class GameDateTests : GameDate
    {
        private GameDate gameDate;

        [SetUp]
        public void SetUp()
        {
            gameDate = new GameDate();
        }

        /// <summary>
        /// Tests the IncrementDay Method, when the week does not change.
        /// Asserts that the new Day and Day of Week is set.
        /// </summary>
        [Test]
        public void IncrementDayTest_NoNewWeek()
        {
            gameDate.IncrementDay();
            Assert.AreEqual(new DateTime(1, 1, 2),gameDate.DateTime);
            Assert.AreEqual(DayOfWeek.Tuesday,gameDate.DayOfWeek);
        }

        /// <summary>
        /// Tests the IncrementDay Method, when the week does change.
        /// Asserts that the new Day and Day of Week is set.
        /// </summary>
        [Test]
        public void IncrementDayTest_NewWeek()
        {
            gameDate.DayOfWeek = DayOfWeek.Sunday;
            gameDate.IncrementDay();
            Assert.AreEqual(DayOfWeek.Monday, gameDate.DayOfWeek);
        }
    }
}
