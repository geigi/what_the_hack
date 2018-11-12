using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEditor.SceneManagement;
using Wth.ModApi.Employees;

public class WthModApiTests {

    [SetUp]
    public void ResetScene()
    {
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
    }
    
    [Test]
    public void WthModApiTestsSimplePasses() {
        // Use the Assert class to test conditions.
    }

    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator WthModApiTestsWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
    
    [Test]
    public void BasicTest()
    {
        GameObject go = new GameObject();
        go.AddComponent<Employee>();
        Assert.IsNotNull(go.GetComponent<Employee>());
    }
}
