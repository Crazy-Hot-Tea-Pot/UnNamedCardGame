using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeTerminalUIController : MonoBehaviour, IPointerClickHandler
{
    private UpgradeController controller;
    private bool isWaintingForInput = false;


    [Header("Intro Screen")]
    public GameObject IntroPanel;
    public TMP_Text IntroConsole;    

    [Header("Heal Upgrade Screen")]
    public GameObject HealthPanel;
    public TMP_Text HealthConsole;

    [Header("Chip Upgrade Screen")]
    public GameObject ChipPrefab;
    public GameObject ChipPanel;    
    public GameObject ChipSelectionPanel;
    public GameObject ChipHolder;

    public TMP_Text ChipConsole;

    [Header("Data Screen")]
    public GameObject DataPanel;
    public TMP_Text DataConsole;
    
    public Vector2 startPosition;    
    public Vector2 endPosition;

    public float TimeForPanelToGetToCenter=2.0f;

    [Header("Error Screen")]
    public GameObject ErrorPanel;
    public TMP_Text ErrorConsole;

    public List<GameObject> allUIPanels;
    public List<TMP_Text> Consoles = new List<TMP_Text>();
    void Awake()
    {
        controller = GameObject.FindGameObjectWithTag("UpgradeController").GetComponent<UpgradeController>();
    }
    void OnEnable()
    {
        controller.OnScreenChanged += UpdateUIScreen;
        controller.OnErrorOccurred += UpdateErrorScreen;
    }

    void OnDisable()
    {
        controller.OnScreenChanged -= UpdateUIScreen;
        controller.OnErrorOccurred -= UpdateErrorScreen;
    }

    /// <summary>
    /// When player clicks one of the links.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if(isWaintingForInput)
            foreach (var console in Consoles)
            {
                // Skip inactive components
                if (console == null || !console.gameObject.activeInHierarchy)
                    continue;

                int linkIndex = TMP_TextUtilities.FindIntersectingLink(console, eventData.position, null);

                if (linkIndex != -1) // If a link was clicked
                {
                    TMP_LinkInfo linkInfo = console.textInfo.linkInfo[linkIndex];
                    string linkID = linkInfo.GetLinkID();

                    // Handle each link based on its ID
                    HandleLinkClick(linkID);
                    return; // Exit after handling the first valid link click
                }
            }
    }
    
    /// <summary>
    /// When UI link is clicked this method is called.
    /// </summary>
    /// <param name="linkID"></param>
    private void HandleLinkClick(string linkID)
    {
        switch (linkID)
        {
            case"UpgradeHealthScreen":

                controller.SwitchToScreen(UpgradeController.Screens.HealthUpgrade);

                break;
            case "UpgradeHealth":

                controller.AttemptToUpgradeHealth();

                break;
            case "UpgradeChipScreen":
                controller.SwitchToScreen(UpgradeController.Screens.ChipUpgrade);
                break;
            case "UpgradeSelectedChip":               
                controller.AttemptToUpgradeChip();
                break;
            case "DataServer":
                controller.SwitchToScreen(UpgradeController.Screens.Data);
                break;
            case "View":
                controller.currentDataMode=UpgradeController.DataMode.View;

                controller.SwitchToScreen(UpgradeController.Screens.Data);
                break;
            case "Save":
                controller.currentDataMode = UpgradeController.DataMode.Save;

                controller.SwitchToScreen(UpgradeController.Screens.Data);
                break;
            case "Load":
                controller.currentDataMode = UpgradeController.DataMode.Load;

                controller.SwitchToScreen(UpgradeController.Screens.Data);
                break;
            case "Back":
                controller.SwitchToScreen(UpgradeController.Screens.Intro);
                break;
            case "Exit":
            case "Exit1":
            case "Exit2":
                controller.SwitchToScreen(UpgradeController.Screens.Exit);

            break;
            default:

            break;
        }
    }

    /// <summary>
    /// Switches to the specified UI screen and deactivates all others.
    /// </summary>
    /// <param name="screen">The UI screen to activate.</param>
    private void UpdateUIScreen(UpgradeController.Screens screen)
    {
        StopAllCoroutines();
        switch (screen)
        {
            case UpgradeController.Screens.Default:
            case UpgradeController.Screens.Exit:
                IntroPanel.SetActive(false);
                HealthPanel.SetActive(false);
                ChipPanel.SetActive(false);
                DataPanel.SetActive(false);
                ErrorPanel.SetActive(false);
                break;
            case UpgradeController.Screens.Intro:

                SetActiveUIElement(IntroPanel);                

                StartCoroutine(RevealText(IntroConsole, true, 0.01f, false,0f,0,false,0));
                break;
            case UpgradeController.Screens.HealthUpgrade:

                SetActiveUIElement(HealthPanel);

                PlayerController tempPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                string tempText = string.Format("Getting <color=#A20000>*Error*</color> Health.\n" +
                "Current Health is <color=#A20000>{0}</color> of Max Health <color=#A20000>{1}</color>.\n" +
                "Will cost <b>150</b> Scrap to Upgrade to <color=#A20000>{2}</color>.\n" +
                "<color=#0000FF><b><u><link=\"UpgradeHealth\">Upgrade</link></u></b></color>\n" +
                "<color=#0000FF><b><u><link=\"Exit\">Exit</link></u></b></color>\n" +
                "<color=#0000FF><u><link=\"Back\">Back</link></u></color>",
                tempPlayer.Health, tempPlayer.MaxHealth, tempPlayer.MaxHealth + 10);

                HealthConsole.SetText(tempText);

                StartCoroutine(RevealText(HealthConsole, false, 0.01f, false, 0f,0,false,0));
                break;
            case UpgradeController.Screens.ChipUpgrade:

                SetActiveUIElement(ChipPanel);

                if (controller.SelectedChip == null)
                {

                    StartCoroutine(RevealText(ChipConsole, true, 0.01f, true,0.1f,5,false,0));
                    StartCoroutine(BringUpChipSelector());
                }
                else
                {
                    ChipConsole.ForceMeshUpdate();

                    ChipSelectionPanel.SetActive(false);

                    ChipConsole.textInfo.Clear();

                    string tempText2 = string.Format("Chip inserted.\n" +
                        "...Loading Chip...\n" +
                        "Chip Info:\n" +
                        "Chip Rarity - {0}\n" +
                        "Chip Name - {1}\n" +
                        "Chip Description - {2}\n" +
                        "Cost to upgrade - <b>{3}</b> Scrap.\n-----\n" +
                        "<color=#0000FF><b><u><link=\"UpgradeSelectedChip\">Upgrade Chip</color></link></u></b>\n" +
                        "<color=#0000FF><b><u><link=\"Exit2\">Exit</color></link></u></b>\n" +
                        "<color=#0000FF><u><link=\"Back\">Back</color></link></u>",
                    controller.SelectedChip.chipRarity, controller.SelectedChip.chipName, controller.SelectedChip.description, controller.SelectedChip.costToUpgrade);



                    ChipConsole.SetText(tempText2);

                    IntroConsole.ForceMeshUpdate();
                    HealthConsole.ForceMeshUpdate();
                    ChipConsole.ForceMeshUpdate();  
                    ErrorConsole.ForceMeshUpdate();



                    StartCoroutine(RevealText(ChipConsole, true, 0.01f, false, 0f,0,false,0));

                }

                break;
            case UpgradeController.Screens.Data:

                SetActiveUIElement(DataPanel);

                string tempDataText="";

                switch (controller.currentDataMode)
                {
                    case UpgradeController.DataMode.Title:
                        tempDataText = string.Format("");
                        break;
                    case UpgradeController.DataMode.View:
                        tempDataText = string.Format("");
                        break;
                    case UpgradeController.DataMode.Save:
                        tempDataText = string.Format("");
                        break;
                    case UpgradeController.DataMode.Load:
                        tempDataText = string.Format("");
                        break;
                }

                DataConsole.SetText(tempDataText);

                StartCoroutine(RevealText(DataConsole,false,0.01f,false,0f,0,false,0f));
                break;
            case UpgradeController.Screens.Error:
                SetActiveUIElement(ErrorPanel);

                StartCoroutine(RevealText(ErrorConsole, false, 0.01f, false, 0f, 0, false, 0f));
                break;                            
            default:
                Debug.LogWarning("There should always be an option here.");
                break;
        }
    }

    /// <summary>
    /// Active Required Panel.
    /// Since dont have multiple canvases and use panels just one line of code to active one panel.
    /// </summary>
    /// <param name="element"></param>
    /// <param name="isActive"></param>
    private void SetActiveUIElement(GameObject targetPanel)
    {
        if(allUIPanels.Count== 0)
        {
            Debug.LogError("Panels are empty!");
        }
        foreach (var panel in allUIPanels)
        {
            panel.SetActive(panel == targetPanel);
        }
    }

    /// <summary>
    /// Error Message.
    /// </summary>
    /// <param name="message"></param>
    private void UpdateErrorScreen(string message)
    {
        ErrorConsole.SetText(message);
    }

    /// <summary>
    /// Populate the panel with Chips first and then,
    /// Bring up window to select Chip.
    /// </summary>
    private IEnumerator BringUpChipSelector()
    {
        isWaintingForInput = false;

        ChipHolder.SetActive(false);

        foreach(NewChip newChip in GameManager.Instance.playerDeck)
        {
            if (newChip.canBeUpgraded)
            {
                GameObject UIChip = Instantiate(ChipPrefab, ChipHolder.transform);

                UIChip.GetComponent<Chip>().newChip = newChip;

                UIChip.GetComponent<Chip>().SetChipModeTo(Chip.ChipMode.WorkShop);                                
            }
            
        }

        ChipHolder.SetActive(true);

        float tempTime = 0;
        RectTransform tempRectTransform = ChipSelectionPanel.GetComponent<RectTransform>();

        while (tempTime < TimeForPanelToGetToCenter)
        {
            tempTime += Time.deltaTime;

            float temp = Mathf.Clamp01(tempTime / TimeForPanelToGetToCenter);

            tempRectTransform.anchoredPosition = Vector2.Lerp(startPosition, endPosition, temp);

            yield return null;
        }

        tempRectTransform.anchoredPosition=endPosition;
    }

    /// <summary>
    /// Reveals text to the UI base on request.
    /// </summary>
    /// <param name="tmpText"></param>
    /// <param name="byLetter"></param>
    /// <param name="revealSpeed">Lower the number faster it appears.</param>
    /// <param name="timeBeforeRestartAnimation"></param>
    /// <returns></returns>
    private IEnumerator RevealText(TMP_Text tmpText, bool byLetter, float revealSpeed, bool blinkText, float blinkDuration, int blinkCount,bool loopAnimation, float timeBeforeRestartAnimation)
    {
        isWaintingForInput = false;

        tmpText.ForceMeshUpdate();

        TMP_TextInfo textInfo = tmpText.textInfo;


        int totalVisibleCharacters = textInfo.characterCount;
        int totalWordCount = textInfo.wordCount;
        int visibleCount = 0;

        bool hasTextChanged = false;
        int counter = 0;
        int currentWord = 0;

        while (true)
        {
            if (hasTextChanged)
            {
                totalVisibleCharacters = textInfo.characterCount;
                hasTextChanged = false;
            }

            if (byLetter)
            {
                if (visibleCount > totalVisibleCharacters)
                {                   
                    if (blinkText)
                        yield return StartCoroutine(BlinkText(tmpText, blinkDuration, blinkCount));

                    if (loopAnimation)
                    {
                        yield return new WaitForSeconds(timeBeforeRestartAnimation); // Pause before restarting
                        visibleCount = 0;
                    }
                    else
                    {
                        isWaintingForInput = true;
                        break;
                    }
                }

                tmpText.maxVisibleCharacters = visibleCount; // Set visible characters
                visibleCount += 1;
                yield return new WaitForSeconds(revealSpeed);
            }
            else
            {
                currentWord = counter % (totalWordCount + 1);

                // Determine visible character count based on the current word
                if (currentWord == 0)
                    visibleCount = 0;
                else if (currentWord < totalWordCount)
                    visibleCount = textInfo.wordInfo[currentWord - 1].lastCharacterIndex + 1;
                else if (currentWord == totalWordCount)
                    visibleCount = totalVisibleCharacters;

                tmpText.maxVisibleCharacters = visibleCount;

                if (visibleCount >= totalVisibleCharacters)
                {
                    if (blinkText)
                        yield return StartCoroutine(BlinkText(tmpText, blinkDuration, blinkCount));

                    if (loopAnimation)
                        yield return new WaitForSeconds(timeBeforeRestartAnimation);
                    else
                    {
                        isWaintingForInput = true;
                        break;
                    }
                }

                counter += 1;
                yield return new WaitForSeconds(revealSpeed * 10); // Adjust speed for word reveal
            }
        }        
    }
    private IEnumerator BlinkText(TMP_Text tmpText, float blinkDuration, int blinkCount)
    {
        int totalVisibleCharacters = tmpText.textInfo.characterCount;
        for (int i = 0; i < blinkCount; i++)
        {
            tmpText.maxVisibleCharacters = 0;
            yield return new WaitForSeconds(blinkDuration);

            tmpText.maxVisibleCharacters = totalVisibleCharacters;
            yield return new WaitForSeconds(blinkDuration);
        }
    }

}