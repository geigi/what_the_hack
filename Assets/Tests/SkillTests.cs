using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
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
            s = new Skill(new SkillDefinition());
        }

        /// <summary>
        /// Tests the AddSkillPoint Method, when the level stays the same.
        /// Asserts that the points are added and the level and aptitude name are the same.
        /// </summary>
        [Test]
        public void AddSkillPointsTest_SameLevel()
        {
            s.AddSkillPoints(10);
            Assert.AreEqual(10, s.points);
            Assert.AreEqual(0, s.level);
            Assert.AreEqual(Skill.LevelAptitudeName.Newbie, s.skillLevelName);
        }

        /// <summary>
        /// Tests the AddSkillPoint Method, when the level advances by one.
        /// Asserts the points are added, the level, nextLevelPoints and aptitude name updated.
        /// </summary>
        [Test]
        public void AddSkillPointsTest_NextLevel()
        {
            s.AddSkillPoints(100);
            Assert.AreEqual(100, s.points);
            Assert.AreEqual(1, s.level);
            Assert.AreEqual(100 * Skill.levelFactor, s.nextLevelPoints);
            Assert.AreEqual(Skill.LevelAptitudeName.Newbie, s.skillLevelName);
        }

        /// <summary>
        /// Tests the AddSkillPoint Method, when the level advances by more than one.
        /// Asserts the points are added, the level, nextLevelPoints and aptitude name updated.
        /// </summary>
        [Test]
        public void AddSkillPointsTest_AdvancedMoreThanOneLevel()
        {
            s.AddSkillPoints(1000);
            Assert.AreEqual(1000, s.points);
            Assert.AreEqual(6, s.level);
            Assert.AreEqual(Skill.LevelAptitudeName.Beginner, s.skillLevelName);
        }
    }
}
