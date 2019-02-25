using UI;
using UnityEngine;

namespace Tools
{
    /// <summary>
    /// Loads a scene asynchronous on start.
    /// </summary>
    public class SceneLoadingProgress : MonoBehaviour
    {
        public ProgressBar ProgressBar;
        
        private void Update()
        {
            ProgressBar.SetProgress(SceneLoaderAsync.Instance.LoadingProgress);
        }
    }
}