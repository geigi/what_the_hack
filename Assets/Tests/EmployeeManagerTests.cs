using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Employees;
using NSubstitute;
using NUnit.Framework;
using NUnit;
using UnityEngine;
using Employees;
using GameTime;
using UnityEngine.TestTools;
using Wth.ModApi.Employees;

namespace Assets.Tests
{
    /// <summary>
    /// Tests for EmployeeManager.
    /// </summary>
    public class EmployeeManagerTests : EmployeeManager
    {
        private EmployeeFactory factory;
        private EmployeeManager emp;
        private ContentHub contentHub;
        private EmployeeData testEmployee;

        [SetUp]
        public void SetUp()
        {
            emp = (EmployeeManager) FormatterServices.GetUninitializedObject(typeof(EmployeeManager));
            EmployeeList list = ScriptableObject.CreateInstance<EmployeeList>();
            factory = Substitute.For<EmployeeFactory>();
            testEmployee = new EmployeeData
            {
                generatedData = new EmployeeGeneratedData {name = "Test Employee"}
            };
            factory.GenerateEmployee().Returns(testEmployee);

            emp.InitDefaultState();
            emp.factoryObject = factory;
            emp.EmployeeForHirePrefab = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        }

        /// <summary>
        /// Tests the GenerateEmployeeForHire Method.
        /// Asserts that an Employee is generated and the generated Employee is returned.
        /// </summary>
        [Test]
        public void GenerateEmployeeForHireTest()
        {
            EmployeeData dat = emp.GenerateEmployeeForHire();
            Assert.AreEqual(testEmployee, dat);
            factory.Received().GenerateEmployee();
        }

        /// <summary>
        /// Tests that the FireEmployee Method, does nothing, when an Employee does not exist.
        /// </summary>
        [Test]
        public void FireEmployee_NonExistingEmp_Test()
        {
            emp.FireEmployee(testEmployee);
            Assert.IsEmpty(emp.GetData().exEmplyoees);
        }

        /// <summary>
        /// Tests that the FireEmployee Method, removes an existing Employee from the hired employee List
        /// and that the Employee is put into exEmployees.
        /// </summary>
        [Test]
        public void FireEmployee_ExistingEmp_Test()
        {
            emp.GetData().hiredEmployees.Add(testEmployee);
            emp.FireEmployee(testEmployee);
            Assert.IsEmpty(emp.GetData().hiredEmployees);
            Assert.IsNotEmpty(emp.GetData().exEmplyoees);
            Assert.AreSame(testEmployee, emp.GetData().exEmplyoees[0]);
        }

        /// <summary>
        /// Tests the DayChangedMethod.
        /// Asserts that a new Employee is put in the EmployeeForHire list and that
        /// AddEmployeeForHireToGui is called.
        /// </summary>
        [Test]
        public void DayChangedTest()
        {
            var manager = Substitute.ForPartsOf<EmployeeManager>();
            manager.InitDefaultState();
            manager.factoryObject = factory;
            manager.WhenForAnyArgs(x => x.AddEmployeeForHireToGui(testEmployee)).DoNotCallBase();
            manager.DayChanged(new GameDate());
            Assert.IsNotEmpty(manager.GetData().employeesForHire);
            manager.Received().AddEmployeeForHireToGui(testEmployee);
        }
        
        /// <summary>
        /// Tests the DayChangedMethod.
        /// Asserts that a new Employee is put in the employeeForHire List and the old one is removed.
        /// Also checks that AddEmployeeForHireToGui and Remove EmployeeForHire are called.
        /// </summary>
        [Test]
        public void DayChangedTest_ListNotEmpty()
        {
            var manager = Substitute.ForPartsOf<EmployeeManager>();
            manager.InitDefaultState();
            manager.factoryObject = factory;
            manager.GetData().employeesForHire.Add(testEmployee);
            manager.EmployeeToGuiMap = new Dictionary<EmployeeData, GameObject> {{testEmployee, new GameObject()}};
            manager.WhenForAnyArgs(x => x.AddEmployeeForHireToGui(Arg.Any<EmployeeData>())).DoNotCallBase();
            manager.WhenForAnyArgs(x => x.RemoveEmployeeForHire(Arg.Any<EmployeeData>())).DoNotCallBase();
            manager.DayChanged(new GameDate());
            Assert.IsNotEmpty(manager.GetData().employeesForHire);
            manager.Received().AddEmployeeForHireToGui(testEmployee);
            manager.Received().RemoveEmployeeForHire(testEmployee);
        }
    }
}
