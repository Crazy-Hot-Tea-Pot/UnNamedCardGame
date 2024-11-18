using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SaveGame(string saveName)
    {
        GameData gameData = new GameData
        {
            health = 100, // Example data; replace with actual data
            maxHealth = 200,
            scrap = 50,
            chipNames = new List<string> { "ChipA", "ChipB" } // Replace with actual playerDeck chips
        };

        DataManager.Instance.Save(saveName, gameData);
    }

    public void LoadGame(string saveName)
    {
        GameData loadedData = DataManager.Instance.RetrieveGameData(saveName);
        if (loadedData != null)
        {
            // Apply loaded data to the game
            Debug.Log($"Loaded Save: {loadedData.saveName} | Time: {loadedData.timeStamp}");
        }
    }
    /// <summary>
    /// Display all save data on screen.
    /// </summary>
    public void DisplaySaves()
    {
        List<GameData> saves = DataManager.Instance.GetAllSaves();
        foreach (var save in saves)
        {
            Debug.Log($"Save Name: {save.saveName} | Time: {save.timeStamp}");
        }
    }
}
