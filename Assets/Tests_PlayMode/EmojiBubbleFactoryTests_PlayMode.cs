using System.Collections;
using Assets.Scripts.Reaction;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Assets.Tests_PlayMode
{
    /// <summary>
    /// Play mode tests for the EmojiBubbleFactory
    /// </summary>
    public class EmojiBubbleFactoryTests_PlayMode
    {
        [SetUp]
        public void SetUp()
        {
            SceneManager.LoadScene("MainGame");
        }

        /// <summary>
        /// Tests the InitReaction function. Asserts that a new GameObject is created, with the EmployeeReaction script.
        /// Also asserts that the variables are correctly set.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator InitReactionTest()
        {
            var bubbleFactory = GameObject.FindWithTag("EmployeeManager").GetComponent<EmojiBubbleFactory>();
            var reaction = bubbleFactory.InitReaction(EmojiBubbleFactory.EmojiType.SUCCESS, Vector3.one, Vector3.zero);
            Assert.AreEqual(1, GameObject.FindWithTag("EmployeeReactions").transform.childCount);
            var reactionScript = reaction.GetComponent<EmployeeReaction>();
            Assert.NotNull(reactionScript);
            Assert.AreEqual(Vector3.zero,reactionScript.offset);
            Assert.AreEqual(Vector3.one, reactionScript.Position);
            Assert.AreEqual("character-Emojis-success", reactionScript.GetComponent<SpriteRenderer>().sprite.name);
            yield return null;
        }

        /// <summary>
        /// Tests the InitReaction function ad asserts that the offset is taken into account.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator InitReactionTest_Offset()
        {
            var bubbleFactory = GameObject.FindWithTag("EmployeeManager").GetComponent<EmojiBubbleFactory>();
            var reaction = bubbleFactory.InitReaction(EmojiBubbleFactory.EmojiType.SUCCESS, Vector3.one, Vector3.one);
            Assert.AreEqual(new Vector3(2, 2, 2), reaction.transform.position);
            yield return null;
        }

        /// <summary>
        /// Tests the NonEmpReaction Method. Asserts a new Object is created and that it is displayed only for the specified time.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator NonEmpReactionTest()
        {
            var bubbleFactory = GameObject.FindWithTag("EmployeeManager").GetComponent<EmojiBubbleFactory>();
            bubbleFactory.NonEmpReaction(EmojiBubbleFactory.EmojiType.SUCCESS, Vector3.one, 4);
            var reaction = GameObject.FindWithTag("EmployeeReactions").transform.GetChild(0);
            Assert.AreEqual(1, GameObject.FindWithTag("EmployeeReactions").transform.childCount);
            yield return new WaitForSeconds(5f);
            Assert.AreEqual(0, GameObject.FindWithTag("EmployeeReactions").transform.childCount);
        }

        /// <summary>
        /// Tests the EmpReaction Method. Asserts the reaction is set for an employee
        /// and that the reaction changes with the position of the employee. 
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator EmpReactionTest()
        {
            var empObj = new GameObject("Employee");
            var emp = empObj.AddComponent<Employee>();
            empObj.SetActive(false);
            var bubbleFactory = GameObject.FindWithTag("EmployeeManager").GetComponent<EmojiBubbleFactory>();
            bubbleFactory.EmpReaction(EmojiBubbleFactory.EmojiType.SUCCESS, emp, Vector3.one, 4);
            var reaction = GameObject.FindWithTag("EmployeeReactions").transform.GetChild(0);
            Assert.IsNotNull(emp.reaction);
            Assert.AreEqual(empObj.transform.position + Vector3.one, reaction.transform.position);
            empObj.SetActive(true);
            empObj.SetActive(false);
            Assert.AreEqual(empObj.transform.position + Vector3.one, reaction.transform.position);
            yield return new WaitForSeconds(5);
            Assert.IsNull(emp.reaction);
        }

        /// <summary>
        /// Asserts that the Fade In and Out Animation works correctly.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator ReactionFadeAnimationTest()
        {
            var bubbleFactory = GameObject.FindWithTag("EmployeeManager").GetComponent<EmojiBubbleFactory>();
            bubbleFactory.NonEmpReaction(EmojiBubbleFactory.EmojiType.ANGRY, Vector3.zero, 4);
            var reaction = GameObject.FindWithTag("EmployeeReactions").transform.GetChild(0);
            yield return new WaitForSeconds(0.2f);
            Assert.IsTrue(1f > reaction.GetComponent<SpriteRenderer>().color.a);
            yield return new WaitForSeconds(1.2f);
            Assert.AreEqual(1, Mathf.RoundToInt(reaction.GetComponent<SpriteRenderer>().color.a));
            yield return new WaitForSeconds(2.2f);
            Assert.IsTrue(1f > reaction.GetComponent<SpriteRenderer>().color.a);
        }
    }
}
