using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Tests
{
    /// <summary>
    /// Test for ScriptableObjectDictionary
    /// </summary>
    class ScriptableObjectDictionaryTests
    {
        private ScriptableObjectDictionary objDictionary;
        private ScriptableObject scriptObj1;
        private ScriptableObject scriptObj2;

        [SetUp]
        public void SetUp()
        {
            objDictionary = ScriptableObject.CreateInstance<ScriptableObjectDictionary>();
            objDictionary.Dictionary = new List<ScriptableObjectDictionary.ScriptableObjectEntry>();
            scriptObj1 = ScriptableObject.CreateInstance<ScriptableObject>();
            scriptObj2 = ScriptableObject.CreateInstance<ScriptableObject>();
        }

        /// <summary>
        /// Tests the GetKey Method.
        /// Asserts that the correct key is returned.
        /// </summary>
        [Test]
        public void GetKeyTest()
        {
            objDictionary.AddUpdate("key", scriptObj1);
            Assert.AreEqual("key", objDictionary.GetKey(scriptObj1));
        }

        /// <summary>
        /// Tests the GetObject Method.
        /// Assert that the correct Object is returned.
        /// </summary>
        [Test]
        public void GetObjectTest()
        {
            objDictionary.AddUpdate("key", scriptObj1);
            Assert.AreSame(scriptObj1, objDictionary.GetObject("key"));
        }

        /// <summary>
        /// Tests the AddUpdate Method, when a new entry is added.
        /// Asserts that the new entry is added.
        /// </summary>
        [Test]
        public void AddUpdateTest_AddKey()
        {
            objDictionary.AddUpdate("key", scriptObj1);
            Assert.IsNotEmpty(objDictionary.Dictionary);
        }

        /// <summary>
        /// Tests the AddUpdateMethod, when an entry is updated to something not null.
        /// Asserts that the right entry was updated.
        /// </summary>
        [Test]
        public void AddUpdateTest_UpdateObjToNotNull()
        {
            objDictionary.AddUpdate("key", scriptObj1);
            objDictionary.AddUpdate("key", scriptObj2);
            Assert.AreSame(scriptObj2,objDictionary.GetObject("key"));
        }

        /// <summary>
        /// Tests the AddUpdate Method, when an entry is updated to null.
        /// Asserts that the entry was not updated.
        /// </summary>
        [Test]
        public void AddUpdateTest_UpdateObjToNull()
        {
            objDictionary.AddUpdate("key", scriptObj1);
            objDictionary.AddUpdate("key", null);
            Assert.AreSame(scriptObj1, objDictionary.GetObject("key"));
        }

        /// <summary>
        /// Tests the Delete Method, when a key is passed as a parameter.
        /// Asserts that the entry is deleted.
        /// </summary>
        [Test]
        public void DeleteTest_ByKey()
        {
            objDictionary.AddUpdate("key", scriptObj1);
            objDictionary.Delete("key");
            Assert.IsNull(objDictionary.GetObject("key"));
        }

        /// <summary>
        /// Tests the Delete Method, when a object is passed as a parameter.
        /// Asserts that the entry is deleted.
        /// </summary>
        [Test]
        public void DeleteTest_ByObject()
        {
            objDictionary.AddUpdate("key", scriptObj1);
            objDictionary.Delete(scriptObj1);
            Assert.IsNull(objDictionary.GetObject("key"));
        }
    }
}
