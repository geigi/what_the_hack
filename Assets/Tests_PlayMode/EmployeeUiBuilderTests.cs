using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.UI.EmployeeWindow;
using Employees;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Wth.ModApi.Employees;

namespace Assets.Tests_PlayMode
{
    /// <summary>
    /// Tests for EmployeeUiBuilder and its Subclasses, as well as SkillUiBuilder.
    /// </summary>
    public class EmployeeUiBuilderTests : EmployeeUiBuilder
    {
        private EmployeeData mockedEmployeeData;
        private HireableEmployeeUiBuilder hireableBuilder;
        private HiredEmployeeUiBuilder hiredBuilder;

        [SetUp]
        public void SetUp()
        {
            mockedEmployeeData = Substitute.For<EmployeeData>();
            mockedEmployeeData.Salary = 100;
            mockedEmployeeData.Level = 2;
            mockedEmployeeData.Prize = 10;
            mockedEmployeeData.Skills = new List<Skill>() { new Skill(ScriptableObject.CreateInstance<SkillDefinition>()) };
            mockedEmployeeData.generatedData = new EmployeeGeneratedData
            {
                name = "surname lastname",
                gender = "male"
            };

            SceneManager.LoadScene("MainGame");

            
        }

        /// <summary>
        /// Tests the SetEmp Method in HireableEmployeeUiBuilder.
        /// Asserts that the correct prize is displayed. Calls the EmployeeUiBuilder Asserts.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator HireableEmployeeUiBuilder_SetEmpTest()
        {
            //Arrange
            var obj = GameObject.FindWithTag("EmployeeManager").GetComponent<EmployeeManager>();
            hireableBuilder = Instantiate(obj.EmployeeForHirePrefab).GetComponent<HireableEmployeeUiBuilder>();
            var func = Substitute.For<UnityAction>();
            //Act
            hireableBuilder.SetEmp(mockedEmployeeData, func);
            /*Aserts*/
            Assert.AreEqual(mockedEmployeeData.Prize + " $", hireableBuilder.prize.text);
            AssertEmployeeUiBuilder(func);
            yield return null;
        }

        /// <summary>
        /// Tests the SetEmp Method in HiredEmployeeUiBuilder.
        /// Asserts that the State Event does work. Calls the EmployeeUiBuilder Asserts.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator HiredEmployeeUiBuilder_SetEmpTest()
        {
            //Arrange
            var obj = GameObject.FindWithTag("EmployeeManager").GetComponent<EmployeeManager>();
            var hiredBuilder = Instantiate(obj.EmployeeHiredPrefab).GetComponent<HiredEmployeeUiBuilder>();
            hiredBuilder.enabled = false;
            var emp = Substitute.For<Employee>();
            emp.EmployeeData = mockedEmployeeData;
            var evt = Substitute.For<UnityEvent>();
            var func = Substitute.For<UnityAction>();
            
            //Act
            hiredBuilder.SetEmp(emp, evt, func);

            /*Assert*/
            AssertEmployeeUiBuilder(func);
            hiredBuilder.employeeState.text = "state";
            evt.Invoke();
            Assert.AreNotEqual("state", hiredBuilder.employeeState.text);
            yield return null;
        }

        /// <summary>
        /// Tests the GenerateSkillGui Method in EmployeeUiBuilder.
        /// Asserts that each skill has its own GameObject
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator GenerateSkillGuiTest()
        {
            //Arrange
            var obj = GameObject.FindWithTag("EmployeeManager").GetComponent<EmployeeManager>().EmployeeForHirePrefab;
            var builder = Instantiate(obj).GetComponent<HireableEmployeeUiBuilder>();
            builder.employeeData = mockedEmployeeData;
            //Act
            builder.GenerateSkillGui();
            //Assert
            Assert.AreEqual(mockedEmployeeData.Skills.Count, builder.skillPanel.transform.childCount);
            yield return null;
        }

        /// <summary>
        /// Test the SetSkill Method in SkillUiBuilder,.
        /// Asserts that the correct sprite, name and level are displayed.
        /// Checks that the level is updated through an Event.
        /// </summary>
        [UnityTest]
        public IEnumerator SetSkillTest()
        {
            //Arrange
            var obj = GameObject.FindWithTag("EmployeeManager").GetComponent<EmployeeManager>().EmployeeForHirePrefab;
            var builder = Instantiate(obj).GetComponent<HireableEmployeeUiBuilder>();
            var skillbuilder = Instantiate(builder.skillPrefab).GetComponent<SkillUIBuilder>();

            Sprite sprite = Sprite.Create(new Texture2D(100, 100), new Rect(1, 1, 2, 3), new Vector2());

            var definition = ScriptableObject.CreateInstance<SkillDefinition>();
            definition.skillName = "A Skill";
            definition.skillSprite = sprite;
            var skill = new Skill(definition);

            skillbuilder.skillEvent = skill.SkillEvent;

            //Act
            skillbuilder.SetSkill(skill);

            //Assert
            Assert.AreSame(sprite, skillbuilder.skillImage.sprite);
            Assert.AreSame(skill.GetName(), skillbuilder.skillName.text);
            Assert.AreEqual(skill.skillLevelName + " " + skill.level, skillbuilder.skillLevel.text);
            skill.AddSkillPoints(1000);
            Assert.AreEqual(skill.skillLevelName + " " + skill.level, skillbuilder.skillLevel.text);
            yield return null;
        }

        /// <summary>
        /// Asserts the things that are handled by the EmplyoeeUiBuilder Parent
        /// </summary>
        /// <param name="func">The Function that should be called when clicking the button</param>
        private void AssertEmployeeUiBuilder(Delegate func)
        {
            // Assert that the name, salary and Specials are displayed correctly
            Assert.AreEqual(mockedEmployeeData.Salary + " $", hireableBuilder.salary.text);
            Assert.AreEqual(mockedEmployeeData.generatedData.name, hireableBuilder.empName.text);
            Assert.IsEmpty(hireableBuilder.specialList.text);
            //Check that the Button invokes the right method
            hireableBuilder.button.onClick.Invoke();
            func.Received(1);
        }
    }
}
