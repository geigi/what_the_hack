using System.Collections;
using System.Collections.Generic;
using Employees.Specials;
using NUnit.Framework;
using Wth.ModApi.Employees;

namespace Assets.Tests
{
    class EmployeeSpecialTests
    {
        private EmployeeData data;
        
        [SetUp]
        public void SetUp()
        {
            data = new EmployeeData();
            data.Specials = new List<EmployeeSpecial>();
        }
        
        [Test]
        public void LearningMultiplier()
        {
            var special = new FastLearner();
            
            data.Specials.Add(special);

            int increment = 10;
            data.IncrementFreeScore(increment);
            Assert.AreEqual(data.FreeScore, increment * (1f + special.GetLearningMultiplier()));
        }
    }
}