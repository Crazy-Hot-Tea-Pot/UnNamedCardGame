using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public class TitleController : MonoBehaviour
{
    [Header("Buttons")]
    public GameObject ButtonPanel;
    public Button PlayButton;
    public Button ResumeButton;
    public Button OptionsButton;
    public Button QuitButton;

    [Header("Button Sounds")]
    public SoundFX ButtonSelectSound;
    public SoundFX ButtonClickSound;

    [Header("Game Status Info")]
    public TextMeshProUGUI VersionText;

    private GameData latestSave = null;

    private List<Button> buttons = new List<Button>();

    // Start is called before the first frame update
    void Start()
    {
        VersionText.SetText("Version: " + Application.version);

        // Add button click listeners
        PlayButton.onClick.AddListener(PlayButtonClickSound);
        PlayButton.onClick.AddListener(() => StartCoroutine(StartGame()));
        ResumeButton.onClick.AddListener(PlayButtonClickSound);
        ResumeButton.onClick.AddListener(() => StartCoroutine(ResumeGame()));
        OptionsButton.onClick.AddListener(PlayButtonClickSound);
        OptionsButton.onClick.AddListener(() => StartCoroutine(OpenOptions()));
        QuitButton.onClick.AddListener(PlayButtonClickSound);
        QuitButton.onClick.AddListener(() => StartCoroutine(Quit()));

        // Add buttons to a list for easier management
        buttons.Add(PlayButton);
        buttons.Add(ResumeButton);
        buttons.Add(OptionsButton);
        buttons.Add(QuitButton);

        // Add OnSelect listeners for each button
        foreach (Button button in buttons)
        {
            AddOnSelectListener(button);
        }


        CheckForSaveData();

    }

    /// <summary>
    /// Plays Sound for when mouse over Button.
    /// 0 references because from inspector.
    /// </summary>
    public void PlayButtonSound()
    {
        SoundManager.PlayFXSound(ButtonSelectSound);
    }
    public void PlayButtonClickSound()
    {
        SoundManager.PlayFXSound(ButtonClickSound);
    }

    public void Credits()
    {
        GameManager.Instance.RequestScene(Levels.Credits);
    }

    /// <summary>
    /// Adds OnSelect listener to play button sound.
    /// </summary>
    private void AddOnSelectListener(Button button)
    {
        var eventTrigger = button.gameObject.GetComponent<UnityEngine.EventSystems.EventTrigger>() ??
                           button.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();

        UnityEngine.EventSystems.EventTrigger.Entry entry = new UnityEngine.EventSystems.EventTrigger.Entry
        {
            eventID = UnityEngine.EventSystems.EventTriggerType.Select
        };

        entry.callback.AddListener((eventData) => PlayButtonSound());
        eventTrigger.triggers.Add(entry);
    }

    /// <summary>
    /// Start a new game.
    /// Set Default Player Stats
    /// </summary>
    private IEnumerator StartGame()
    {

        // Wait for the duration of the sound (or a short delay)
        yield return new WaitForSeconds(1f);

        GameData startData = new GameData();

        startData.SaveName = "Beginning";
        startData.Level = Levels.Tutorial;
        startData.MaxHealth = 50;
        startData.Health = 50;
        startData.Scraps = 100;
        startData.TimeStamp = DateTime.Now;


        // Adds gear to list.
        
        foreach (Item gear in GearManager.Instance.StartingGear)
        {
            Item gearInstance = Instantiate(gear);
            GearManager.Instance.Acquire(gearInstance);
            GearManager.Instance.EquipGear(gearInstance); 
        }

        // Add Starting Chips
        foreach(NewChip newChip in ChipManager.Instance.StartingChips)
        {
            NewChip chipInstance = Instantiate(newChip);

            ChipManager.Instance.AddNewChipToDeck(chipInstance);
        }
        
        DataManager.Instance.CurrentGameData=startData;

        DataManager.Instance.Save(startData.SaveName);

        GameManager.Instance.RequestScene(Levels.Tutorial);

    }
    /// <summary>
    /// Logic for resuming the game.
    /// </summary>
    private IEnumerator ResumeGame()
    {
        // Wait for the duration of the sound (or a short delay)
        yield return new WaitForSeconds(1f);

        // Load the save data into the game
        DataManager.Instance.LoadData(latestSave.SaveName);

        // Request the scene from GameManager
        GameManager.Instance.RequestScene(latestSave.Level);
    }
    private IEnumerator OpenOptions()
    {
        // Wait for the duration of the sound (or a short delay)
        yield return new WaitForSeconds(1f);
    }
    /// <summary>
    /// Quit Game
    /// </summary>
    private IEnumerator Quit()
    {
        // Wait for the duration of the sound (or a short delay)
        yield return new WaitForSeconds(1f);

        Application.Quit();
    }
    /// <summary>
    /// Get all saves from DataManager
    /// Enable or disable the ResumeButton based on save availability
    /// </summary>
    private void CheckForSaveData()
    {    
        List<GameData> allSaves = DataManager.Instance.GetAllSaves();

        if (allSaves != null && allSaves.Count > 0)
            ResumeButton.interactable = true;        
        else
            ResumeButton.interactable = false;

        // Find the latest save by timestamp        
        foreach (var save in allSaves)
        {
            if (latestSave == null || save.TimeStamp > latestSave.TimeStamp)
                latestSave = save;
        }
    }
    [ContextMenu("Bring in Buttons")]
    private void BringInButtons()
    {
        ButtonPanel.GetComponent<Animator>().SetTrigger("BringInButtons");
    }

    private void OnDestroy()
    {
        // Remove listeners to avoid issues when reloading scenes
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
            var eventTrigger = button.gameObject.GetComponent<UnityEngine.EventSystems.EventTrigger>();
            if (eventTrigger != null)
            {
                eventTrigger.triggers.Clear();
            }
        }
    }
}
