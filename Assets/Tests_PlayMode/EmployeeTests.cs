using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Wth.ModApi.Employees;

namespace Assets.Tests_PlayMode
{
    /// <summary>
    /// Tests for the Employee class.
    /// </summary>
    class EmployeeTests : Employee
    {
        private EmployeeData generatedEmpData;
        private EmployeeData specialEmpData;

        private AnimationClip idle;
        private AnimationClip walking;
        private AnimationClip working;


        [SetUp]
        public void SetUp()
        {
            idle = new AnimationClip();
            idle.AddEvent(new AnimationEvent {functionName = "ShadowEvent"});
            walking = new AnimationClip();
            walking.AddEvent(new AnimationEvent { functionName = "ShadowEvent" });
            working = new AnimationClip();
            working.AddEvent(new AnimationEvent { functionName = "ShadowEvent" });

            generatedEmpData = Substitute.For<EmployeeData>();
            generatedEmpData.generatedData = new EmployeeGeneratedData
            {
                name = "surname lastname",
                gender = "female",
                idleClipIndex = 1,
                walkingClipIndex = 4,
                workingClipIndex = 8
            };

            specialEmpData = Substitute.For<EmployeeData>();
            specialEmpData.EmployeeDefinition = ScriptableObject.CreateInstance<EmployeeDefinition>();
            specialEmpData.EmployeeDefinition.IdleAnimation = idle;
            specialEmpData.EmployeeDefinition.WalkingAnimation = walking;
            specialEmpData.EmployeeDefinition.WorkingAnimation = working;

            SceneManager.LoadScene("MainGame");
        }

        /// <summary>
        /// Tests the Init Method of the EmployeeClass.
        /// Asserts that the right Animations are applied and the Event Function is correctly set.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator InitTest_GeneratedEmployee()
        {
            var obj = new GameObject();
            var emp = obj.AddComponent<Employee>();
            emp.enabled = false;
            emp.init(generatedEmpData, true);
            /*Asserts*/
            Assert.IsNotNull(obj.GetComponent<SpriteRenderer>());
            Assert.AreSame(ContentHub.Instance.femaleAnimationClips[1] , emp.animator.runtimeAnimatorController.animationClips[1]);
            Assert.AreSame(ContentHub.Instance.femaleAnimationClips[4], emp.animator.runtimeAnimatorController.animationClips[0]);
            Assert.AreSame(ContentHub.Instance.femaleAnimationClips[8], emp.animator.runtimeAnimatorController.animationClips[2]);
            Assert.AreEqual("SetSpriteThroughScript", emp.animator.runtimeAnimatorController.animationClips[0].events[0].functionName);
            Assert.AreEqual("SetSpriteThroughScript", emp.animator.runtimeAnimatorController.animationClips[1].events[0].functionName);
            Assert.AreEqual("SetSpriteThroughScript", emp.animator.runtimeAnimatorController.animationClips[2].events[0].functionName);

            yield return null;
        }

        [UnityTest]
        public IEnumerator InitAndSetAnimationEventFunctionTest_SpecialEmployee()
        {
            var obj = new GameObject();
            var emp = obj.AddComponent<Employee>();
            emp.enabled = false;
            emp.init(specialEmpData, true);
            Assert.IsNotNull(obj.GetComponent<SpriteRenderer>());
            Assert.AreSame(idle, emp.animator.runtimeAnimatorController.animationClips[1]);
            Assert.AreSame(walking, emp.animator.runtimeAnimatorController.animationClips[0]);
            Assert.AreSame(working, emp.animator.runtimeAnimatorController.animationClips[2]);
            Assert.AreEqual("SetSpriteThroughScript", emp.animator.runtimeAnimatorController.animationClips[0].events[0].functionName);
            Assert.AreEqual("SetSpriteThroughScript", emp.animator.runtimeAnimatorController.animationClips[1].events[0].functionName);
            Assert.AreEqual("SetSpriteThroughScript", emp.animator.runtimeAnimatorController.animationClips[2].events[0].functionName);
            yield return null;
        }
    }
}
