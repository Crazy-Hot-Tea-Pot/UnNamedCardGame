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
    }

    void OnDisable()
    {
        controller.OnScreenChanged -= UpdateUIScreen;
    }

    void Start()
    {
        
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
                PlayerController tempPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

                if(tempPlayer.Scrap >= 150)
                {
                    // made this bank for later.
                    var Bank = tempPlayer.TakeScrap(150);

                    tempPlayer.UpgradeMaxHealth(10);

                    //Display new info
                    controller.SwitchToScreen(UpgradeController.Screens.HealthUpgrade);
                }
                else
                {
                    // TODO: Error Scree stuff
                    ErrorConsole.SetText("Not enough scrap to upgrade health.");
                    Debug.Log("Not enough scrap to upgrade health."); // Placeholder log
                }
                break;
            case "UpgradeChipScreen":
                controller.SwitchToScreen(UpgradeController.Screens.ChipUpgrade);
                break;
            case "UpgradeSelectedChip":               
                controller.AttemptToUpgradeChip();
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
                IntroPanel.SetActive(false);
                HealthPanel.SetActive(false);
                ChipPanel.SetActive(false);
                break;
            case UpgradeController.Screens.Intro:

                SetActiveUIElement(IntroPanel);                

                StartCoroutine(RevealText(IntroConsole, true, 0.01f, false,0f,0,false,0));
                break;
            case UpgradeController.Screens.HealthUpgrade:

                SetActiveUIElement(HealthPanel);

                PlayerController tempPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                string tempText = string.Format("Getting <#A20000>*Error*</color> Health.\n" +
                "Current Health is <#A20000>{0}</color> of Max Health <#A20000>{1}</color>.\n" +
                "Will cost <b>150</b> Scrap to Upgrade to <#A20000>{2}</color>.\n" +
                "<b><u><link=\"UpgradeHealth\">Upgrade</link></u></b>\n" +
                "<b><u><link=\"Exit1\">Exit</link></u></b>",
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
                        "<b><u><link=\"UpgradeSelectedChip\">Upgrade Chip</link></u></b>\n\n" +
                        "<b><u><link=\"Exit2\">Exit</link></u></b>",
                        controller.SelectedChip.chipRarity, controller.SelectedChip.chipName, controller.SelectedChip.description, controller.SelectedChip.costToUpgrade);



                    ChipConsole.SetText(tempText2);

                    IntroConsole.ForceMeshUpdate();
                    HealthConsole.ForceMeshUpdate();
                    ChipConsole.ForceMeshUpdate();  
                    ErrorConsole.ForceMeshUpdate();



                    StartCoroutine(RevealText(ChipConsole, true, 0.01f, false, 0f,0,false,0));

                }

                break;
            case UpgradeController.Screens.Exit:
                IntroPanel.SetActive(false);
                HealthPanel.SetActive(false);
                ChipPanel.SetActive(false);
                break;
            default:
                IntroPanel.SetActive(false);
                HealthPanel.SetActive(false);
                ChipPanel.SetActive(false);
                break;
        }
    }

    /// <summary>
    /// Active Required Panel.
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
    /// Populate the panel with chips first and then,
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
                ChipPrefab.GetComponent<Chip>().newChip = newChip;


                ChipPrefab.GetComponent<Chip>().IsInWorkShop = true;


                GameObject UIChip = Instantiate(ChipPrefab, ChipHolder.transform);
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
                    isWaintingForInput = true;
                    if (blinkText)
                        yield return StartCoroutine(BlinkText(tmpText, blinkDuration, blinkCount));

                    if (loopAnimation)
                    {
                        yield return new WaitForSeconds(timeBeforeRestartAnimation); // Pause before restarting
                        visibleCount = 0;
                    }
                    else
                        break;                    
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
                        break;
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