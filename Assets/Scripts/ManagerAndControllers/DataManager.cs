using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public string saveName;
    //Level the player is on
    public string Level;
    public int health;
    public int maxHealth;
    public int scrap;
    // Save chips by their names
    public List<string> chipNames = new List<string>();
    // Save Abilities by their name
    public List<string> abilityNames = new List<string>();
    //time of save
    public string timeStamp;
}
public class DataManager : MonoBehaviour
{

    public static DataManager Instance
    {
        get;
        private set;
    }

    public GameData GameData
    {
        get
        {
            return gameData;
        }
        set
        {
            gameData = value;
        }
    }

    private string saveDirectory;


    [SerializeField]
    private GameData gameData;



    void Awake()
    {
        // Check if another instance of the SettingsManager exists
        if (Instance == null)
        {
            Instance = this;

            // Keep this object between scenes
            DontDestroyOnLoad(gameObject);

            //Save Directory
            saveDirectory = Path.Combine(Application.persistentDataPath, "PlayerSaveData");

            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            gameData = new GameData();

            //Try to load data first.
            LoadData("AutoSave");
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicates
        }       
    }
    /// <summary>
    /// Saves all Data
    /// </summary>
    public void Save(string saveName)
    {
        if (GameManager.Instance.InCombat)
        {
            Debug.LogWarning("Can not save while in combat");
            return;
        }
        else
        {

            string saveFilePath = Path.Combine(saveDirectory, $"{saveName}.json");
            gameData.saveName = saveName;
            gameData.timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            string json = JsonUtility.ToJson(gameData, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log($"Game saved successfully: {saveFilePath}");
        }
    }
    public void Save(string saveName, GameData gameData)
    {
        string saveFilePath = Path.Combine(saveDirectory, $"{saveName}.json");
        gameData.saveName = saveName;
        gameData.timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Game saved successfully: {saveFilePath}");
    }
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

        //Load player stats
        GameData.health = saveData.health;
        GameData.maxHealth = saveData.maxHealth;
        GameData.scrap = saveData.scrap;

        // Load chips
        GameManager.Instance.playerDeck.Clear();
        foreach (var chipName in saveData.chipNames)
        {
            NewChip chip = Resources.Load<NewChip>($"Scriptables/Chips/{chipName}");
            if (chip != null)
            {
                GameData.chipNames.Add(chipName);
                GameManager.Instance.playerDeck.Add(chip);
                Debug.Log($"Loaded chip: {chip.chipName}");
            }
            else
            {
                Debug.LogWarning($"Chip {chipName} not found in Resources/Scriptables/Chips.");
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
}
