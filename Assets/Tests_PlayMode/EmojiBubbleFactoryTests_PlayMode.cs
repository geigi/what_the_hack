using System.Collections;
using Assets.Scripts.Reaction;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Assets.Tests_PlayMode
{
    public class EmojiBubbleFactoryTests_PlayMode
    {
        [SetUp]
        public void SetUp()
        {
            SceneManager.LoadScene("MainGame");
        }

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

        [UnityTest]
        public IEnumerator InitReactionTest_Offset()
        {
            var bubbleFactory = GameObject.FindWithTag("EmployeeManager").GetComponent<EmojiBubbleFactory>();
            var reaction = bubbleFactory.InitReaction(EmojiBubbleFactory.EmojiType.SUCCESS, Vector3.one, Vector3.one);
            Assert.AreEqual(new Vector3(2, 2, 2), reaction.transform.position);
            yield return null;
        }

        [UnityTest]
        public IEnumerator NonEmpReactionTest()
        {
            var bubbleFactory = GameObject.FindWithTag("EmployeeManager").GetComponent<EmojiBubbleFactory>();
            bubbleFactory.NonEmpReaction(EmojiBubbleFactory.EmojiType.SUCCESS, Vector3.one, 4);
            Assert.AreEqual(1, GameObject.FindWithTag("EmployeeReactions").transform.childCount);
            yield return new WaitForSeconds(5);
            Assert.AreEqual(0, GameObject.FindWithTag("EmployeeReactions").transform.childCount);
        }

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
    }
}
