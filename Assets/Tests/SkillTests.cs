using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Tests
{
    /// <summary>
    /// Tests for the Skill Class.
    /// </summary>
    class SkillTests
    {
        private Skill s;

        [SetUp]
        public void SetUp()
        {
            s = new Skill(ScriptableObject.CreateInstance<SkillDefinition>());
        }

        /// <summary>
        /// Tests the AddSkillPoint Method, when the level stays the same.
        /// Asserts that the points are added and the level and aptitude name are the same.
        /// </summary>
        [Test]
        public void AddSkillPointsTest_SameLevel()
        {
            s.AddSkillPoints(1);
            Assert.AreEqual(1, s.Points);
            Assert.AreEqual(1, s.Level);
            Assert.AreEqual(Skill.LevelAptitudeName.Newbie, s.SkillLevelName);
        }

        /// <summary>
        /// Tests the AddSkillPoint Method, when the level advances by one.
        /// Asserts the points are added, the level, nextLevelPoints and aptitude name updated.
        /// </summary>
        [Test]
        public void AddSkillPointsTest_NextLevel()
        {
            var nextLevelPoints = s.nextLevelPoints;
            s.AddSkillPoints(s.nextLevelPoints);
            Assert.AreEqual(nextLevelPoints, s.Points + s.SpendPoints);
            Assert.AreEqual(2, s.Level);
            Assert.AreEqual((float) Math.Pow(Skill.levelFactor, s.Level), s.nextLevelPoints);
            Assert.AreEqual(Skill.LevelAptitudeName.Newbie, s.SkillLevelName);
        }

        /// <summary>
        /// Tests the AddSkillPoint Method, when the level advances by more than one.
        /// Asserts the points are added, the level, nextLevelPoints and aptitude name updated.
        /// </summary>
        [Test]
        public void AddSkillPointsTest_AdvancedMoreThanOneLevel()
        {
            s.AddSkillPoints(1000);
            Assert.AreEqual(1000, (int) (s.Points + s.SpendPoints));
            Assert.AreEqual(15, s.Level);
            Assert.AreEqual(Skill.LevelAptitudeName.Master, s.SkillLevelName);
        }
    }
}
