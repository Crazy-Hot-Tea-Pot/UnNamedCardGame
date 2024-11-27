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
    /// Sort auto-saves by number (not timestamp).
    /// Find the next auto-save number (fill gaps or cycle).
    /// Get all used auto-save numbers.
    /// Find the lowest unused number, or cycle to the oldest save.
    /// Delete the oldest save to maintain the limit.
    /// then finally save the data.
    /// </summary>
    public void AutoSave()
    {
        List<GameData> allSaves = GetAllSaves();
        List<GameData> autoSaves = allSaves.FindAll(save => save.SaveName.StartsWith("AutoSave"));

        autoSaves.Sort((a, b) =>
            int.Parse(a.SaveName.Replace("AutoSave", "")) - int.Parse(b.SaveName.Replace("AutoSave", ""))
        );

        int maxAutoSaves = SettingsManager.Instance.DataSettings.MaxAutoSave;

        // Start with 1 by default
        int nextAutoSaveNumber = 1;
        if (autoSaves.Count > 0)
        {
            var usedNumbers = autoSaves.Select(save => int.Parse(save.SaveName.Replace("AutoSave", ""))).ToList();

            if (usedNumbers.Count < maxAutoSaves)
            {
                nextAutoSaveNumber = Enumerable.Range(1, maxAutoSaves).Except(usedNumbers).First();
            }
            else
            {
                nextAutoSaveNumber = int.Parse(autoSaves[0].SaveName.Replace("AutoSave", ""));
                DeleteSave(autoSaves[0].SaveName);
                autoSaves.RemoveAt(0);
            }
        }

        string saveName = $"AutoSave{nextAutoSaveNumber}";

        // Save the data
        Save(saveName);
    }

    /// <summary>
    /// Load Player stats
    /// Re-link Chips by loading from Resources
    /// Re-link Gears(havn't been done yet)
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

        // load Gears
        GameManager.Instance.Items.Clear();

        foreach(var item in saveData.Gears)
        {
            Gear gear = Resources.Load<Gear>($"Scriptables/Equipment/{item.GearName}");

            Gear newGear = Instantiate(gear);

            newGear.IsEquipted=item.isEquipped;



            if (item.AmountOfAbilities > 0)
            {
                for (int i = 0; i < item.AmountOfAbilities; i++)
                {
                    if(item.ListOfAbilities[i].AbilityName == newGear.AbilityList[i].abilityName)
                    {
                        newGear.AbilityList[i].isUpgraded = item.ListOfAbilities[i].IsUpgraded;
                    }
                }
            }

            GameManager.Instance.Items.Add(newGear);
        }

        Debug.Log("Game loaded successfully.");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="saveName"></param>
    /// <returns></returns>
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
        Debug.LogWarning($"Attempted to delete a save that does not exist: {saveName}");
        return false;
    }
}
