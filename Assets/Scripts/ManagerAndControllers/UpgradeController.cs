using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    public GameObject ChipPrefab;
    public UpgradeTerminalUIController UIController;

    [Header("Screens In Game World")]
    public GameObject DefaultScreen;
    public GameObject IntroScreen;
    public GameObject HealthUpgradeScreen;
    public GameObject ChipUpgradeScreen;

    public enum Screens
    {
        Default,
        Intro,
        HealthUpgrade,
        ChipUpgrade,
        Exit
    }

    //Current Screen Active in game world
    private Screens currentScreen;

    private bool hasTextChanged;

    void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchToScreen(Screens.Default);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Switches to the specified screen and deactivates the current screen.
    /// </summary>
    /// <param name="screen">The screen type to switch to.</param>
    public void SwitchToScreen(Screens screen)
    {
        StopAllCoroutines();

        switch (screen)
        {
            case Screens.Default:                              

                IntroScreen.SetActive(false);
                DefaultScreen.SetActive(true);
                HealthUpgradeScreen.SetActive(false);
                //ChipUpgradeScreen.SetActive(false);

                StartCoroutine(PlayDefaultScreen());

                UIController.SwitchDisplay(Screens.Exit); ;
                currentScreen = screen;
                break;
            case Screens.Intro:

                IntroScreen.SetActive(true);
                DefaultScreen.SetActive(false);
                HealthUpgradeScreen?.SetActive(false);
                //ChipUpgradeScreen.SetActive(false);

                StartCoroutine(PlayIntroScreen());

                UIController.SwitchDisplay(screen);

                currentScreen = screen;
                break;
             case Screens.HealthUpgrade:

                IntroScreen.SetActive(false);
                DefaultScreen.SetActive(false);
                HealthUpgradeScreen?.SetActive(true);
                //ChipUpgradeScreen.SetActive(false);

                PlayerController tempPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

                string tempText = string.Format("Getting <#A20000> *Error*</color> Health.\n" +
            "Current Health is <#A20000>{0}</color> of Max Health <#A20000>{1}</color>.\n" +
            "Will cost <b>150</b> Scrap to Upgrade to <#A20000>{2}</color>.\n" +
            "<link=\"UpgradeHealth\"><b><u>Upgrade</link>\n" +
            "<link=\"Exit\">Exit</b></u></link>",
            tempPlayer.Health, tempPlayer.MaxHealth, tempPlayer.MaxHealth + 10);

                HealthUpgradeScreen.GetComponent<TextMeshPro>().SetText(tempText);

                StartCoroutine(PlayHealthScreen());

                currentScreen = screen;
                break;
            case Screens.ChipUpgrade:

                IntroScreen.SetActive(false);
                DefaultScreen.SetActive(false);
                HealthUpgradeScreen?.SetActive(false);
                //ChipUpgradeScreen.SetActive(true);

                currentScreen = screen;

                break;

            case Screens.Exit:

                IntroScreen.SetActive(false);
                DefaultScreen.SetActive(true);
                HealthUpgradeScreen?.SetActive(false);
                //ChipUpgradeScreen.SetActive(false);

                StartCoroutine(PlayDefaultScreen());

                currentScreen = screen;
                break;
            default:
                break;
        }
    }
    private void OnTextChanged(UnityEngine.Object obj)
    {
        hasTextChanged = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SwitchToScreen(Screens.Intro);            
            other.GetComponent<PlayerController>().IsInteracting = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            SwitchToScreen(Screens.Default);           
            other.GetComponent<PlayerController>().IsInteracting = false;
        }
    }

    /// <summary>
    /// Plays default text in loop
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayDefaultScreen()
    {

        DefaultScreen.GetComponent<TextMeshPro>().ForceMeshUpdate();

        TMP_TextInfo textInfo = DefaultScreen.GetComponent<TextMeshPro>().textInfo;

        int totalVisibleCharacters = textInfo.characterCount;
        int visibleCount = 0;

        while (true)
        {
            if (hasTextChanged)
            {
                totalVisibleCharacters = textInfo.characterCount;
                hasTextChanged = false;
            }

            if (visibleCount > totalVisibleCharacters)
            {
                //Blinking effect 3 times before resetting
                for (int i = 0; i < 3; i++)  
                {
                    // Hide text
                    DefaultScreen.GetComponent<TextMeshPro>().maxVisibleCharacters = 0;
                    
                    yield return new WaitForSeconds(1f);

                    // Show full text
                    DefaultScreen.GetComponent<TextMeshPro>().maxVisibleCharacters = totalVisibleCharacters;

                    yield return new WaitForSeconds(1f);
                }

                yield return new WaitForSeconds(10.0f);
                visibleCount = 0;
            }

            DefaultScreen.GetComponent<TextMeshPro>().maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            visibleCount += 1;

            yield return new WaitForSeconds(0.01f);
        }
    }
    /// <summary>
    /// Plays Intro Text in Game World
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayIntroScreen()
    {

        IntroScreen.GetComponent<TextMeshPro>().ForceMeshUpdate();

        TMP_TextInfo textInfo = IntroScreen.GetComponent<TextMeshPro>().textInfo;

        int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
        int visibleCount = 0;

        while (visibleCount != totalVisibleCharacters)
        {
            if (hasTextChanged)
            {
                totalVisibleCharacters = textInfo.characterCount; // Update visible character count.
                hasTextChanged = false;
            }

            if (visibleCount > totalVisibleCharacters)
            {
                yield return new WaitForSeconds(1.0f);
                visibleCount = 0;
            }

            IntroScreen.GetComponent<TextMeshPro>().maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            visibleCount += 1;

            yield return new WaitForSeconds(0.1f);
        }
    }
    /// <summary>
    /// Method revealing the text one word at a time.
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayHealthScreen()
    {
        HealthUpgradeScreen.GetComponent<TextMeshPro>().ForceMeshUpdate();

        int totalWordCount = HealthUpgradeScreen.GetComponent<TextMeshPro>().textInfo.wordCount;
        int totalVisibleCharacters = HealthUpgradeScreen.GetComponent<TextMeshPro>().textInfo.characterCount; // Get # of Visible Character in text object
        int counter = 0;
        int currentWord = 0;
        int visibleCount = 0;

        while (visibleCount != totalVisibleCharacters)
        {
            currentWord = counter % (totalWordCount + 1);

            // Get last character index for the current word.
            if (currentWord == 0) // Display no words.
                visibleCount = 0;
            else if (currentWord < totalWordCount) // Display all other words with the exception of the last one.
                visibleCount = HealthUpgradeScreen.GetComponent<TextMeshPro>().textInfo.wordInfo[currentWord - 1].lastCharacterIndex + 1;
            else if (currentWord == totalWordCount) // Display last word and all remaining characters.
                visibleCount = totalVisibleCharacters;

            HealthUpgradeScreen.GetComponent<TextMeshPro>().maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            // Once the last character has been revealed, wait the set speed and start over.
            if (visibleCount >= totalVisibleCharacters)
            {
                yield return new WaitForSeconds(1.0f);
            }

            counter += 1;

            yield return new WaitForSeconds(0.1f);
        }
    }
}
