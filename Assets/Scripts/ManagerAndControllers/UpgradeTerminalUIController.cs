using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeTerminalUIController : MonoBehaviour, IPointerClickHandler
{
    private UpgradeController controller;


    [Header("Intro Screen")]
    public GameObject IntroPanel;
    public TMP_Text IntroConsole;    
    private bool hasTextChanged;

    [Header("Heal Upgrade Screen")]
    public GameObject HealthPanel;
    public TMP_Text HealthConsole;

    [Header("Chip Upgrade Screen")]
    public GameObject ChipPanel;
    public TMP_Text ChipConsole;

    [Header("Error Screen")]
    public GameObject ErrorPanel;
    public TMP_Text ErrorConsole;

    public List<TMP_Text> Consoles = new List<TMP_Text>();

    void OnEnable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
    }

    void OnDisable()
    {
        TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
    }

    void Start()
    {
        controller=GameObject.FindGameObjectWithTag("UpgradeController").GetComponent<UpgradeController>();
    }
    /// <summary>
    /// Switches to the specified UI screen and deactivates all others.
    /// </summary>
    /// <param name="screen">The UI screen to activate.</param>
    public void SwitchDisplay(UpgradeController.Screens screen)
    {
        StopAllCoroutines();
        switch (screen)
        {
            case UpgradeController.Screens.Default:
                break;
            case UpgradeController.Screens.Intro:
                HealthPanel.SetActive(false);
                IntroPanel.SetActive(true);
                //ChipPanel.SetActive(false);
                StartCoroutine(RevealIntroScreen());
                break;
            case UpgradeController.Screens.HealthUpgrade:

                IntroPanel.SetActive(false);
                HealthPanel.SetActive(true);
                //ChipPanel.SetActive(false);

                PlayerController tempPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                
                string tempText = string.Format("Getting <#A20000> *Error*</color> Health.\n" +
            "Current Health is <#A20000>{0}</color> of Max Health <#A20000>{1}</color>.\n" +
            "Will cost <b>150</b> Scrap to Upgrade to <#A20000>{2}</color>.\n" +
            "<link=\"UpgradeHealth\"><b><u>Upgrade</link>\n" +
            "<link=\"Exit\">Exit</b></u></link>",
            tempPlayer.Health, tempPlayer.MaxHealth, tempPlayer.MaxHealth + 10);

                HealthConsole.SetText(tempText);

                StartCoroutine(RevealHealthScreen());
                break;
            case UpgradeController.Screens.ChipUpgrade:

                IntroPanel.SetActive(false);
                HealthPanel.SetActive(false);
                //ChipPanel.SetActive(true);

                break;
            case UpgradeController.Screens.Exit:

                IntroPanel.SetActive(false);
                HealthPanel.SetActive(false);
                //ChipPanel.SetActive(false);

                break;
        }
    }

    /// <summary>
    /// Is called when the text is changed
    /// </summary>
    /// <param name="obj"></param>
    private void OnTextChanged(UnityEngine.Object obj)
    {
        hasTextChanged = true;
    }

    /// <summary>
    /// When player clicks one of the links.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (var console in Consoles)
        {
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(console, eventData.position, null);

            if (linkIndex != -1) // A link was clicked
            {
                TMP_LinkInfo linkInfo = console.textInfo.linkInfo[linkIndex];
                string linkID = linkInfo.GetLinkID();

                HandleLinkClick(linkID);
                break; // Exit loop after handling the first clicked link
            }
        }
    }
    private void HandleLinkClick(string linkID)
    {
        switch (linkID)
        {
            case"UpgradeHealthScreen":

                SwitchDisplay(UpgradeController.Screens.HealthUpgrade);

                controller.SwitchToScreen(UpgradeController.Screens.HealthUpgrade);

                break;
            case "UpgradeHealth":
                PlayerController tempPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

                if(tempPlayer.Scrap >= 150)
                {
                    // made this bank for later.
                    var Bank = tempPlayer.TakeScrap(150);

                    tempPlayer.UpgradeMaxHealth(10);

                    //Display new info
                    SwitchDisplay(UpgradeController.Screens.HealthUpgrade);
                    controller.SwitchToScreen(UpgradeController.Screens.HealthUpgrade);
                }
                else
                {
                    // TODO: Error Scree stuff
                    Debug.Log("Not enough scrap to upgrade health."); // Placeholder log
                }
                break;
            case "UpgradeChip":

                break;
            case "Exit":
                SwitchDisplay(UpgradeController.Screens.Exit);

                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().IsInteracting = false;

                controller.SwitchToScreen(UpgradeController.Screens.Exit);

                break;                
        }
    }

    /// <summary>
    /// Method revealing the text one character at a time for intro screen.
    /// </summary>
    /// <returns></returns>
    IEnumerator RevealIntroScreen()
    { 

        IntroConsole.ForceMeshUpdate();       

        TMP_TextInfo textInfo = IntroConsole.textInfo;

        int totalVisibleCharacters = textInfo.characterCount; // Get # of Visible Character in text object
        int visibleCount = 0;

       while (visibleCount!=totalVisibleCharacters)
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

            IntroConsole.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

            visibleCount += 1;

            yield return new WaitForSeconds(0.01f);
       }
    }


    /// <summary>
    /// Method revealing the text one word at a time.
    /// </summary>
    /// <returns></returns>
    IEnumerator RevealHealthScreen()
    {
        HealthConsole.ForceMeshUpdate();

        int totalWordCount = HealthConsole.textInfo.wordCount;
        int totalVisibleCharacters = HealthConsole.textInfo.characterCount; // Get # of Visible Character in text object
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
                visibleCount = HealthConsole.textInfo.wordInfo[currentWord - 1].lastCharacterIndex + 1;
            else if (currentWord == totalWordCount) // Display last word and all remaining characters.
                visibleCount = totalVisibleCharacters;

            HealthConsole.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

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