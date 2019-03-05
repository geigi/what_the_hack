using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

namespace Assets.Tests_PlayMode
{
    class BalanceChangedListenerTests
    {
        [SetUp]
        public void SetUp()
        {
            SceneManager.LoadScene("MainGame");
        }

        /// <summary>
        /// Tests the Animation coroutine. Asserts that the animation works correctly, when the player loses money.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator AnimationTest_LostMoney()
        {
            var changedListener = GameObject.Find("MoneyText").GetComponent<BalanceChangedListener>();
            changedListener.ChangeBalance(3000);
            yield return new WaitForSeconds(0.1f);
            Assert.AreNotEqual("4000", changedListener.text.text);
            Assert.AreNotEqual("3000", changedListener.text.text);
            yield return new WaitForSeconds(5f);
            Assert.AreEqual("3000", changedListener.text.text);
        }

        /// <summary>
        /// Tests the animation coroutine. Asserts that the animation works correctly, when the player gaines money.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator AnimationTest_GainedMoney()
        {
            var changedListener = GameObject.Find("MoneyText").GetComponent<BalanceChangedListener>();
            changedListener.ChangeBalance(5000);
            yield return new WaitForSeconds(0.1f);
            Assert.AreNotEqual("5000", changedListener.text.text);
            Assert.AreNotEqual("4000", changedListener.text.text);
            yield return new WaitForSeconds(6f);
            Assert.AreEqual("5000", changedListener.text.text);
        }

        /// <summary>
        /// Tests the animation coroutine. Asserts that only a single instance of the animation is running at any given time.
        /// </summary>
        /// <returns></returns>
        [UnityTest]
        public IEnumerator AnimationTest_MultipleInstances()
        {
            var changedListener = GameObject.Find("MoneyText").GetComponent<BalanceChangedListener>();
            changedListener.ChangeBalance(5000);
            yield return new WaitForSeconds(0.1f);
            Assert.AreNotEqual("5000", changedListener.text.text);
            Assert.AreNotEqual("4000", changedListener.text.text);
            changedListener.ChangeBalance(4500);
            yield return new WaitForSeconds(6f);
            Assert.AreNotEqual("5000", changedListener.text.text);
            Assert.AreNotEqual("4500", changedListener.text.text);
            yield return new WaitForSeconds(5f);
            Assert.AreEqual("4500", changedListener.text.text);
        }
    }
}
