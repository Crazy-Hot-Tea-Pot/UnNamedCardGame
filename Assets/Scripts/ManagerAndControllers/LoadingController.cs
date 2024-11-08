using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    private string targetScene;

    public Image ProgressBar;
    // Start is called before the first frame update
    void Start()
    {
        targetScene = GameManager.Instance.TargetScene.ToString();
        StartCoroutine(LoadSceneAsync(targetScene));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Start loading the target scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // Prevents auto-activation

        // Display of progress goes here.
        ProgressBar.fillAmount = Mathf.Clamp01(asyncLoad.progress / 1f);

        // Wait until the scene is fully loaded (progress reaches 90%)
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 1f)
            {
                // Scene is almost ready
                // add a delay or later we can add delay :D
                // or wait for user input to continue
                // Short delay for smoother transition
                yield return new WaitForSeconds(10f);

                // Activate the scene
                asyncLoad.allowSceneActivation = true;
            }

            // Wait for next frame
            yield return null;
        }
    }
}