using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
public class DataManager : MonoBehaviour
{    
    public static DataManager Instance
    {
        get;
        private set;
    }

    /// <summary>
    /// currentGameData for current game session.
    /// </summary>
    public GameData CurrentGameData
    {
        get
        {
            return currentGameData;
        }
        set
        {
            currentGameData = value;
        }
    }

    [SerializeField]
    private string saveDirectory;


    [SerializeField]
    private GameData currentGameData;



    void Awake()
    {
        // Check if another instance of the SettingsManager exists
        if (Instance == null)
        {
            Instance = this;

            // Keep this object between scenes
            DontDestroyOnLoad(gameObject);

            //Save Directory
            saveDirectory = Path.Combine(Application.dataPath, "GameData");

            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }          
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicates
        }    
        
        CurrentGameData = new GameData();
    }
    /// <summary>
    /// Saves all CurrentGameData.
    /// Make sure to update the CurrentGameData first.
    /// </summary>
    public void Save(string saveName)
    {
        string saveFilePath = Path.Combine(saveDirectory, $"{saveName}.json");

        CurrentGameData.SaveName = saveName;
        CurrentGameData.TimeStamp = DateTime.Now;
        CurrentGameData.UpdateTimeStamp();

        string json = JsonUtility.ToJson(CurrentGameData, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log($"Game saved successfully: {saveFilePath}");

    }

    /// <summary>
    /// AutoSave.
    /// Get all current auto-saves.
    /// Filter to only auto-saves.
    /// Determine the next auto-save number.
    /// Construct the save name.
    /// </summary>
    public void AutoSave()
    {
        List<GameData> allSaves = GetAllSaves();
        List<GameData> autoSaves = allSaves.FindAll(save => save.SaveName.StartsWith("AutoSave"));

        // Handle the auto-save limit
        HandleAutoSaveLimit(autoSaves);

        // Get the next auto-save number after sorting
        int nextAutoSaveNumber = autoSaves.Count > 0
            ? autoSaves.Max(save => int.Parse(save.SaveName.Replace("AutoSave", ""))) + 1
            : 1;

        string saveName = $"AutoSave{nextAutoSaveNumber}";

        // Save the data
        Save(saveName);

    }

    /// <summary>
    /// Load Player stats
    /// Re-link Chips by loading from Resources
    /// Re-link Abilities(havn't been done yet)
    /// </summary>
    /// <param name="saveName"></param>
    public void LoadData(string saveName)
    {
        string saveFilePath = Path.Combine(saveDirectory, $"{saveName}.json");

        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning($"Save file not found: {saveName}");
            return;
        }

        string json = File.ReadAllText(saveFilePath);
        GameData saveData = JsonUtility.FromJson<GameData>(json);

        // Parse the string back into a DateTime object
        saveData.ParseTimeStamp();
        CurrentGameData = saveData;

        //Load player stats
        CurrentGameData.Health = saveData.Health;
        CurrentGameData.MaxHealth = saveData.MaxHealth;
        CurrentGameData.Scraps = saveData.Scraps;


        // Reconstruct the player's deck
        GameManager.Instance.playerDeck.Clear();
        foreach (var chipSave in saveData.Chips)
        {
            NewChip baseChip = Resources.Load<NewChip>($"Scriptables/Chips/{chipSave.Name}");
            if (baseChip != null)
            {
                // Create a copy and apply the saved state
                NewChip loadedChip = Instantiate(baseChip);
                if(loadedChip.canBeUpgraded)
                    loadedChip.IsUpgraded = chipSave.IsUpgraded;
                else
                    loadedChip.IsUpgraded = false;

                loadedChip.DisableCounter = chipSave.DisableCounter;
                GameManager.Instance.playerDeck.Add(loadedChip);
            }
            else
            {
                Debug.LogWarning($"Chip {chipSave.Name} not found in Resources.");
            }
        }

        // load Abilities
        //TODO

        Debug.Log("Game loaded successfully.");
    }

    public GameData RetrieveGameData(string saveName)
    {
        string saveFilePath = Path.Combine(saveDirectory, $"{saveName}.json");
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning($"Save file not found: {saveName}");
            return null;
        }

        string json = File.ReadAllText(saveFilePath);
        GameData gameData = JsonUtility.FromJson<GameData>(json);
        Debug.Log($"Game loaded successfully: {saveFilePath}");
        return gameData;
    }

    /// <summary>
    /// Gets all the saves the player has made.
    /// </summary>
    /// <returns></returns>
    public List<GameData> GetAllSaves()
    {
        List<GameData> saves = new List<GameData>();

        if (!Directory.Exists(saveDirectory)) 
            return saves;

        string[] saveFiles = Directory.GetFiles(saveDirectory, "*.json");

        foreach (string filePath in saveFiles)
        {
            string json = File.ReadAllText(filePath);
            GameData gameData = JsonUtility.FromJson<GameData>(json);
            saves.Add(gameData);
        }

        return saves;
    }

    /// <summary>
    /// Delete Save Data.
    /// </summary>
    /// <param name="saveName">Must be exact name of the Save</param>
    /// <returns></returns>
    public bool DeleteSave(string saveName)
    {
        string savePath = Path.Combine(saveDirectory, $"{saveName}.json");
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            
            Debug.Log($"Deleted save: {saveName}");

            return true;
        }
        return false;
    }

    /// <summary>
    /// Delete any old AutoSaves
    /// </summary>
    private void HandleAutoSaveLimit(List<GameData> autoSaves)
    {
        // Sort auto-saves by timestamp (oldest first)
        autoSaves.Sort((a, b) => a.TimeStamp.CompareTo(b.TimeStamp));

        // Check if the number of auto-saves exceeds the limit
        while (autoSaves.Count >= SettingsManager.Instance.DataSettings.MaxAutoSave)
        {
            // Delete the oldest save
            GameData oldestSave = autoSaves[0];
            string oldestFilePath = Path.Combine(saveDirectory, $"{oldestSave.SaveName}.json");

            if (File.Exists(oldestFilePath))
            {
                File.Delete(oldestFilePath);
                Debug.Log($"Deleted old auto-save: {oldestSave.SaveName}");
            }

            // Remove the oldest save from the list
            autoSaves.RemoveAt(0);
        }
    }
}
