using System.Collections;
using Base;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderAsync : Singleton<SceneLoaderAsync> {

    // Loading Progress: private setter, public getter
    private float _loadingProgress;
    public float LoadingProgress => _loadingProgress;

    public void LoadScene()
    {
        // kick-off the one co-routine to rule them all
        StartCoroutine(LoadScenesInOrder());
    }

    private IEnumerator LoadScenesInOrder()
    {
        // LoadSceneAsync() returns an AsyncOperation, 
        // so will only continue past this point when the Operation has finished
        yield return SceneManager.LoadSceneAsync("Loading");

        // as soon as we've finished loading the loading screen, start loading the game scene
        yield return StartCoroutine(LoadScene("MainGame"));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        var asyncScene = SceneManager.LoadSceneAsync(sceneName);

        // this value stops the scene from displaying when it's finished loading
        asyncScene.allowSceneActivation = false;

        while (!asyncScene.isDone)
        {
            // loading bar progress
            _loadingProgress = Mathf.Clamp01(asyncScene.progress / 0.9f);

            // scene has loaded as much as possible, the last 10% can't be multi-threaded
            if (asyncScene.progress >= 0.9f)
            {
                // we finally show the scene
                asyncScene.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}