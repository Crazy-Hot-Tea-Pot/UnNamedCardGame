using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameData;
//using static UnityEditor.Progress;

public class TitleController : MonoBehaviour
{
    [Header("Buttons")]
    public Button PlayButton;
    public Button ResumeButton;
    public Button OptionsButton;
    public Button QuitButton;

    [Header("Game Status Info")]
    public TextMeshProUGUI VersionText;

    private GameData latestSave = null;
    // Start is called before the first frame update
    void Start()
    {
        VersionText.SetText("Version: " + Application.version);

        // Add listeners to buttons
        PlayButton.onClick.AddListener(StartGame);
        ResumeButton.onClick.AddListener(ResumeGame);
        OptionsButton.onClick.AddListener(OpenOptions);
        QuitButton.onClick.AddListener(Quit);

        CheckForSaveData();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Plays Sound for when mouse over Button.
    /// 0 references because from inspector.
    /// </summary>
    public void PlayButtonSound()
    {
        SoundManager.PlayFXSound(SoundFX.MenuSelectionSound);
    }
    /// <summary>
    /// Start a new game.
    /// Set Default player Stats
    /// </summary>
   private void StartGame()
   {
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
            GearManager.Instance.Acquire(gear);
            GearManager.Instance.EquipGear(gear); 
        }

        // Add Starting Chips
        foreach(NewChip newChip in ChipManager.Instance.StartingChips)
        {
            startData.Chips.Add(new ChipData
            {
                Name=newChip.chipName,
                IsUpgraded=false,
                DisableCounter=newChip.DisableCounter
            });
        }

        ChipManager.Instance.PlayerDeck.AddRange(ChipManager.Instance.StartingChips);
        
        DataManager.Instance.CurrentGameData=startData;

        DataManager.Instance.Save(startData.SaveName);

        GameManager.Instance.RequestScene(Levels.Tutorial);

    }
    /// <summary>
    /// Logic for resuming the game.
    /// </summary>
    private void ResumeGame()
    {
        // Load the save data into the game
        DataManager.Instance.LoadData(latestSave.SaveName);

        // Request the scene from GameManager
        GameManager.Instance.RequestScene(latestSave.Level);
    }
    private void OpenOptions()
    {

    }
    /// <summary>
    /// Quit Game
    /// </summary>
    private void Quit()
    {
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
}
