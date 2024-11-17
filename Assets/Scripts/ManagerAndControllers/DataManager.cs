using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int health;
    public int maxHealth;
    public int scrap;
    // Save chips by their names
    public List<string> chipNames = new List<string>();
    // Save Abilities by their name
    public List<string> abilityNames = new List<string>();
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

    /// <summary>
    /// If Found game data.
    /// </summary>
    public bool SaveFileFound
    {
        get
        {
            return saveFileFound;
        }
        private set
        {
            saveFileFound = value;
        }
    }

    private string saveFilePath;

    private bool saveFileFound;

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

            // Set the save file path, for some reason we doing different than industry standard.
            saveFilePath = Path.Combine(Application.dataPath, "GameData.json");

            gameData = new GameData();

            //Try to load data first.
            LoadData();
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicates
        }       
    }
    /// <summary>
    /// Saves all Data
    /// </summary>
    public void Save()
    {
        if (GameManager.Instance.InCombat)
        {
            Debug.LogWarning("Can not save while in combat");
            return;
        }
        else
        {

            string json = JsonUtility.ToJson(GameData, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log("Game saved successfully to: " + saveFilePath);
        }
    }
    public void LoadData()
    {
        if (!File.Exists(saveFilePath))
        {
            SaveFileFound = false;
            Debug.LogWarning("Save file not found.");
            return;
        }

        SaveFileFound = true;

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

        Debug.Log("Game loaded successfully.");
    }
}
