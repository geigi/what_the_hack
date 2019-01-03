using System;
using UnityEngine;
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
    public class EmployeeFactoryTests
    {

        private EmployeeFactory factory;
        Material mat;

        [SetUp]
        public void SetUp()
        {
            factory = (EmployeeFactory) FormatterServices.GetUninitializedObject(typeof(EmployeeFactory));
            factory.Awake();
        }

        [Test]
        public void GenerateMaterialTest()
        {
            Material material = factory.GenerateMaterial();
            Assert.AreNotEqual(EmployeeFactory.defaultHair, material.GetColor("_HairColor"));
            Assert.AreNotEqual(EmployeeFactory.defaultSkin, material.GetColor("_SkinColor"));
            Assert.AreNotEqual(EmployeeFactory.defaultEyes, material.GetColor("_EyeColor"));
            Assert.AreNotEqual(EmployeeFactory.defaultShirt, material.GetColor("_ShirtColor"));
            Assert.AreNotEqual(EmployeeFactory.defaultShoes, material.GetColor("_ShoeColor"));
            Assert.AreNotEqual(EmployeeFactory.defaultShorts, material.GetColor("_ShortsColor"));
        }

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

        [Test]
        public void GenerateColorTest()
        {
            var enumValues = Enum.GetValues(typeof(EmployeePart));
            System.Random rnd = new System.Random();
            var part = (EmployeePart) enumValues.GetValue(rnd.Next(enumValues.Length));
            Color32 col = factory.GenerateColor(part);
            Assert.IsTrue(factory.GetCurrentDictionary(part).ContainsKey(col));
        }

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
