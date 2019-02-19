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
        public void LevelUp()
        {
            s.LevelUp();
            Assert.AreEqual(2, s.Level);
            Assert.AreEqual(Skill.LevelAptitudeName.Newbie, s.SkillLevelName);
        }
    }
}
