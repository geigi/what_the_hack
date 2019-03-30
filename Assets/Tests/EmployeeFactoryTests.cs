using System;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Employees;
using GameTime;
using Missions;
using Mono.Cecil.Cil;
using NSubstitute;
using Team;
using UnityEditor.SceneManagement;
using UnityEngine.Rendering;
using Wth.ModApi.Employees;
using Wth.ModApi.Items;
using Wth.ModApi.Missions;
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
            var teamManger = Substitute.For<TeamManager>();
            var gameTime = Substitute.For<GameTime.GameTime>();
            var missionManager = Substitute.For<MissionManager>();
            missionManager.GetData().ReturnsForAnyArgs(new MissionManagerData()
            {
                Completed = new List<Mission>()
            });

            factory.gameTime = gameTime;
            factory.teamManager = teamManger;
            factory.missionManager = missionManager;
        }

        /// <summary>
        /// Test the Generate Material Method.
        /// Asserts that each Material Color is Set to a Color from the specific Dictionary.s 
        /// </summary>
        [Test]
        public void GenerateMaterialTest()
        {
            Material material = factory.GenerateMaterial();
            Texture2D tex = material.GetTexture("_SwapTex") as Texture2D;
            Assert.IsTrue(EmployeeFactory.hairColors.Keys.Contains<Color32>(tex.GetPixel((int)EmployeeFactory.SwapIndex.hair, 0)));
            Assert.IsTrue(EmployeeFactory.skinColors.Keys.Contains<Color32>(tex.GetPixel((int)EmployeeFactory.SwapIndex.skin, 0)));
            Assert.IsTrue(EmployeeFactory.eyesColors.Keys.Contains<Color32>(tex.GetPixel((int)EmployeeFactory.SwapIndex.eyes, 0)));
            Assert.IsTrue(EmployeeFactory.shirtColors.Keys.Contains<Color32>(tex.GetPixel((int)EmployeeFactory.SwapIndex.shirt, 0)));
            Assert.IsTrue(EmployeeFactory.shoesColors.Keys.Contains<Color32>(tex.GetPixel((int)EmployeeFactory.SwapIndex.shoes, 0)));
            Assert.IsTrue(EmployeeFactory.shortsColors.Keys.Contains<Color32>(tex.GetPixel((int)EmployeeFactory.SwapIndex.shorts, 0)));
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
            var tex = mat.GetTexture("_SwapTex") as Texture2D;
            Assert.AreEqual((Color) emp.eyeColor, tex.GetPixel((int)EmployeeFactory.SwapIndex.eyes, 0));
            Assert.AreEqual((Color) emp.hairColor, tex.GetPixel((int)EmployeeFactory.SwapIndex.hair, 0));
            Assert.AreEqual((Color) emp.shirtColor, tex.GetPixel((int)EmployeeFactory.SwapIndex.shirt, 0));
            Assert.AreEqual((Color) emp.shoeColor, tex.GetPixel((int)EmployeeFactory.SwapIndex.shoes, 0));
            Assert.AreEqual((Color) emp.shortsColor, tex.GetPixel((int)EmployeeFactory.SwapIndex.shorts, 0));
            Assert.AreEqual((Color) emp.skinColor, tex.GetPixel((int)EmployeeFactory.SwapIndex.skin, 0));
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
        /// Test the GenerateName Method.
        /// Asserts that the name is correctly set.
        /// </summary>
        [Test]
        public void GenerateNameTest()
        {
            var generatedData = new EmployeeGeneratedData() { gender = "male"};
            var names = Substitute.For<NameLists>();
            names.PersonName(PersonNames.MaleFirstName).Returns("male");
            names.PersonName(PersonNames.FemaleFirstName).Returns("female");
            names.PersonName(PersonNames.LastName).Returns("last");
            factory.names = names;
            factory.GenerateName(ref generatedData);
            Assert.AreEqual("male last", generatedData.name);
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

        [Test]
        public void ConditionsMetTest_True()
        {
            factory.teamManager.calcGameProgress().ReturnsForAnyArgs(3);
            factory.gameTime.GetData().ReturnsForAnyArgs(new GameTimeData()
                { Date = new GameDate() { DateTime = new DateTime(1, 1, 20) } });

            var mission = new MissionDefinition();
            mission.MissionHooks = new MissionHookList();
            mission.MissionHooks.MissionHooks = new List<MissionHook>();

            var empDef = Substitute.For<EmployeeDefinition>();
            empDef.SpawnWhenAllConditionsAreMet = true;
            empDef.GameProgress = 2;
            empDef.NumberOfDaysTillEmpCanSpawn = 10;
            empDef.MissionSucceeded = new MissionDefinition[] {mission};
            factory.missionManager.GetData().Completed.Add(new Mission(mission));
            empDef.ItemsBought = new ItemDefinition[0];

            Assert.True(factory.ConditionsMet(empDef));
        }

        [Test]
        public void ConditionsMetTest_TrueNoConditionsSet()
        {
            var empDef = Substitute.For<EmployeeDefinition>();
            empDef.SpawnWhenAllConditionsAreMet = false;
            Assert.True(factory.ConditionsMet(empDef));
        } 

        [Test]
        public void ConditionsMetTest_FalseGameProgress()
        {
            factory.teamManager.calcGameProgress().ReturnsForAnyArgs(2);
            factory.gameTime.GetData().ReturnsForAnyArgs(new GameTimeData()
                { Date = new GameDate() { DateTime = new DateTime(1, 1, 1) } });

            var empDef = Substitute.For<EmployeeDefinition>();
            empDef.SpawnWhenAllConditionsAreMet = true;
            empDef.GameProgress = 10;
            empDef.MissionSucceeded = new MissionDefinition[0];
            empDef.ItemsBought = new ItemDefinition[0];
            Assert.False(factory.ConditionsMet(empDef));
        }

        [Test]
        public void ConditionsMetTest_FalseNumDays()
        {
            factory.teamManager.calcGameProgress().ReturnsForAnyArgs(0);
            factory.gameTime.GetData().ReturnsForAnyArgs(new GameTimeData()
                {Date = new GameDate() {DateTime = new DateTime(1, 1, 10)}});

            var empDef = Substitute.For<EmployeeDefinition>();
            empDef.SpawnWhenAllConditionsAreMet = true;
            empDef.GameProgress = 0;
            empDef.MissionSucceeded = new MissionDefinition[0];
            empDef.ItemsBought = new ItemDefinition[0];
            empDef.NumberOfDaysTillEmpCanSpawn = 11;

            Assert.False(factory.ConditionsMet(empDef));
        }

        [Test]
        public void ConditionsMetTest_FalseMissionNotSucceeded()
        {
            factory.teamManager.calcGameProgress().ReturnsForAnyArgs(0);
            factory.gameTime.GetData().ReturnsForAnyArgs(new GameTimeData()
                { Date = new GameDate() { DateTime = new DateTime(1, 1, 1) } });

            var mission = new MissionDefinition();

            var empDef = Substitute.For<EmployeeDefinition>();
            empDef.SpawnWhenAllConditionsAreMet = true;
            empDef.GameProgress = 0;
            empDef.MissionSucceeded = new MissionDefinition[] {mission};
            empDef.ItemsBought = new ItemDefinition[0];
            empDef.NumberOfDaysTillEmpCanSpawn = 0;

            Assert.False(factory.ConditionsMet(empDef));
        }

        [Test]
        public void AddSpecialEmployeesTest()
        {
            var SpecialEmployees = new List<EmployeeDefinition>
            {
                new EmployeeDefinition()
                {
                    SpawnWhenAllConditionsAreMet = true,
                    GameProgress = 10,
                    NumberOfDaysTillEmpCanSpawn = 0,
                    MissionSucceeded = new MissionDefinition[0],
                    ItemsBought = new ItemDefinition[0],
                    Recurring = true
                },
                new EmployeeDefinition()
                {
                    SpawnWhenAllConditionsAreMet = true,
                    GameProgress = 0,
                    NumberOfDaysTillEmpCanSpawn = 0,
                    MissionSucceeded = new MissionDefinition[0],
                    ItemsBought = new ItemDefinition[0],
                    Recurring = true
                },
                new EmployeeDefinition()
                {
                    SpawnWhenAllConditionsAreMet = true,
                    GameProgress = 0,
                    NumberOfDaysTillEmpCanSpawn = 0,
                    MissionSucceeded = new MissionDefinition[0],
                    ItemsBought = new ItemDefinition[0],
                    Recurring = false
                },
                new EmployeeDefinition()
                {
                    SpawnWhenAllConditionsAreMet = true,
                    GameProgress = 0,
                    NumberOfDaysTillEmpCanSpawn = 0,
                    MissionSucceeded = new MissionDefinition[0],
                    ItemsBought = new ItemDefinition[0],
                    Recurring = true
                }
            };

            var contentHub = Substitute.For<ContentHub>();
            contentHub.DefaultSpecialEmployees = new EmployeeList() {employeeList = SpecialEmployees};
            var empManager = new EmployeeManager();
            Debug.LogWarning(empManager);
            empManager.InitDefaultState();
            empManager.data.employeesForHire.Add(new EmployeeData(){EmployeeDefinition = SpecialEmployees[0]});
            empManager.data.hiredEmployees.Add(new EmployeeData(){EmployeeDefinition = SpecialEmployees[1]});
            empManager.data.exEmplyoees.Add(new EmployeeData(){EmployeeDefinition = SpecialEmployees[2]});
            empManager.data.exEmplyoees.Add(new EmployeeData() { EmployeeDefinition = SpecialEmployees[3]});
            var factory = Substitute.ForPartsOf<EmployeeFactory>();
            factory.contentHub = contentHub;
            factory.employeeManager = empManager;
            factory.ConditionsMet(new EmployeeDefinition()).ReturnsForAnyArgs(true);
            factory.AddSpecialEmployees();
            factory.ReceivedWithAnyArgs(4).ConditionsMet(Arg.Any<EmployeeDefinition>());
            Assert.AreEqual(1, factory.specialEmployeesToSpawn.Count);
            Assert.AreSame(SpecialEmployees[3], factory.specialEmployeesToSpawn[0]);
        }
    }
}
