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
        }
        
        [Test]
        public void LearningMultiplier()
        {
            var special = new FastLearner();
            
            data.AddSpecial(special);

            int increment = 10;
            data.IncrementFreeScore(increment);
            Assert.AreEqual(data.FreeScore, increment * (1f + special.GetLearningMultiplier()));
        }

        [Test]
        public void CriticalSuccessFactor()
        {
            var special = new LuckyDevil();
            
            data.AddSpecial(special);
            
            Assert.AreEqual(data.CriticalSuccessChance, 1 + special.GetCriticalSuccessChance());
        }
        
        [Test]
        public void CriticalFailureFactor()
        {
            var special = new Risky();
            
            data.AddSpecial(special);
            
            Assert.AreEqual(data.CriticalFailureChance, 1 + special.GetCriticalFailureChance());
        }
    }
}