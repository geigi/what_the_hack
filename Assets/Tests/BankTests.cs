using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Employees;
using NSubstitute;
using NSubstitute.Exceptions;
using NUnit.Framework;
using UE.Events;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Tests
{
    /// <summary>
    /// Tests for the Bank class.
    /// </summary>
    class BankTests : Bank
    {
        private Bank bank;
        private BalanceChangedListener listener;
        private Action<int> act;

        [SetUp]
        public void SetUp()
        {
            bank = (Bank) FormatterServices.GetUninitializedObject(typeof(Bank));
            bank.balance = 1000;
            act = Substitute.For<Action<int>>();
            bank.balanceChanged = new IntEvent();
            bank.balanceChanged.AddListener(new UnityAction<int>(act));
        }

        /// <summary>
        /// Test the Pay Method. Asserts, that when the player has enough money the method returns true,
        /// the balance changes and the event is invoked. 
        /// </summary>
        [Test]
        public void PayTest_ReturnsTrue()
        {
            Assert.IsTrue(bank.Pay(100));
            Assert.AreEqual(900, bank.balance);
            act.Received(1);
        }

        /// <summary>
        /// Test the Pay Method. Asserts, that when the player has not enough money the method returns false,
        /// the balance does not changes and the event is not invoked. 
        /// </summary>

        [Test]
        public void PayTest_ReturnsFalse()
        {
            Assert.IsFalse(bank.Pay(1100));
            Assert.AreEqual(1000, bank.balance);
            act.Received(0);
        }

        /// <summary>
        /// Tests the Income Method. Asserts that the money is added to the balance and the event is invoked.
        /// </summary>
        [Test]
        public void IncomeTest()
        {
            bank.Income(100);
            Assert.AreEqual(1100, bank.balance);
            act.Received(1);
        }
    }
}
