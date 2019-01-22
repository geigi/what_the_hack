using System;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEditor.SceneManagement;
using Wth.ModApi.Employees;
using Wth.ModApi.Names;
using Random = System.Random;

namespace Assets.Tests
{
    /// <summary>
    /// Tests for EmployeeFactory.
    /// </summary>
    public class EmployeeFactoryTests
    {

        private EmployeeFactory factory;
        Material mat;

        [SetUp]
        public void SetUp()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/MainGame.unity");
            factory = new EmployeeFactory();
        }

        /// <summary>
        /// Test the Generate Material Method.
        /// Asserts that each Material Color is Set to a Color from the specific Dictionary.s 
        /// </summary>
        [Test]
        public void GenerateMaterialTest()
        {
            Material material = factory.GenerateMaterial();
            Assert.IsTrue(EmployeeFactory.hairColors.Keys.Contains<Color32>(material.GetColor("_HairColor")));
            Assert.IsTrue(EmployeeFactory.skinColors.Keys.Contains<Color32>(material.GetColor("_SkinColor")));
            Assert.IsTrue(EmployeeFactory.eyesColors.Keys.Contains<Color32>(material.GetColor("_EyeColor")));
            Assert.IsTrue(EmployeeFactory.shirtColors.Keys.Contains<Color32>(material.GetColor("_ShirtColor")));
            Assert.IsTrue(EmployeeFactory.shoesColors.Keys.Contains<Color32>(material.GetColor("_ShoeColor")));
            Assert.IsTrue(EmployeeFactory.shortsColors.Keys.Contains<Color32>(material.GetColor("_ShortsColor")));
        }

        /// <summary>
        /// Tests the GenerateColorForEmployeeMethod.
        /// Asserts that all Colors are correctly set.
        /// </summary>
        [Test]
        public void GenerateColorForEmployeeTest()
        {
            var emp = GenerateMockedEmployee();
            Material mat = factory.GenerateMaterialForEmployee(emp);
            Assert.AreEqual((Color) emp.eyeColor, mat.GetColor("_EyeColor"));
            Assert.AreEqual((Color) emp.hairColor, mat.GetColor("_HairColor"));
            Assert.AreEqual((Color) emp.shirtColor, mat.GetColor("_ShirtColor"));
            Assert.AreEqual((Color) emp.shoeColor, mat.GetColor("_ShoeColor"));
            Assert.AreEqual((Color) emp.shortsColor, mat.GetColor("_ShortsColor"));
            Assert.AreEqual((Color) emp.skinColor, mat.GetColor("_SkinColor"));
        }

        /// <summary>
        /// Tests the GenerateSkill Method.
        /// Asserts that the "All Purpose" Skill is generated and that there are no duplicate skills.
        /// </summary>
        [Test]
        public void GenerateSkillsTest()
        {
            var skills = factory.GenerateSkills();
            //Every Employee needs the All Purpose Skill
            Assert.True(skills.Exists((skill) => skill.GetName().Equals("All Purpose")));
            //No Skill Duplicates
            var hash = new HashSet<String>();
            Assert.IsFalse(skills.Any(skill => !hash.Add(skill.GetName())));
        }

        /// <summary>
        /// Tests the GenerateColor Method.
        /// Assert that the Color Generated for an EmployeePart corresponds to a color in the specific Dictionary.
        /// </summary>
        [Test]
        public void GenerateColorTest()
        {
            var enumValues = Enum.GetValues(typeof(EmployeePart));
            foreach (EmployeePart empPart in enumValues)
            {
                Color32 col = factory.GenerateColor(empPart);
                Assert.IsTrue(factory.GetCurrentDictionary(empPart).ContainsKey(col));
            }
        }

        /// <summary>
        /// Generates a Mocked Employee.
        /// </summary>
        /// <returns>The Mocked Employee</returns>
        private EmployeeGeneratedData GenerateMockedEmployee()
        {
            EmployeeGeneratedData genData = new EmployeeGeneratedData();
            genData.AssignRandomGender();
            var employeeParts = Enum.GetValues(typeof(EmployeePart));
            foreach (EmployeePart part in employeeParts)
            {
                genData.SetColorToPart(factory.GenerateColor(part), part);
            }

            return genData;
        }
    }
}
