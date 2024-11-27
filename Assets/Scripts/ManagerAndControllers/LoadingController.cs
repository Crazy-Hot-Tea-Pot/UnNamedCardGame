using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    private string targetScene;
    private NewChip choosenChip;
    

    public GameObject ChipDisplay;
    public TextMeshProUGUI ChipTip;
    public Image ProgressBar;

    [Header("Loading screen settings")]
    public float rotationSpeed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        targetScene = GameManager.Instance.TargetScene.ToString();
        StartCoroutine(LoadSceneAsync(targetScene));

        choosenChip = GameManager.Instance.playerDeck[Random.Range(0, GameManager.Instance.playerDeck.Count)];

        ChipDisplay.GetComponent<Chip>().newChip = choosenChip;
        ChipTip.SetText("Chip Tip: "+choosenChip.ChipTip);

        ChipDisplay.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Gradually rotate the image on the Y-axis
        ChipDisplay.GetComponent<Transform>().Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Start loading the target scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // Prevents auto-activation

        float elapsedTime = 0f;
        float FillDuration = 5f;
        while(elapsedTime < FillDuration)
        {
            elapsedTime += Time.deltaTime;
            ProgressBar.fillAmount = Mathf.Lerp(0,1,elapsedTime/FillDuration);
            yield return null;
        }

        // Wait until the scene is fully loaded (progress reaches 90%)
        while (!asyncLoad.isDone)
        {
            // Display of progress goes here.
            //ProgressBar.fillAmount = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (asyncLoad.progress >= 0.9f)
            {
                // Scene is almost ready
                // add a delay or later we can add delay :D
                // or wait for user input to continue
                // Short delay for smoother transition
                yield return new WaitForSeconds(1f);

                // Activate the scene
                asyncLoad.allowSceneActivation = true;
            }

            // Wait for next frame
            yield return null;
        }
    }
}