using System.Collections.Generic;
using System;

/// <summary>
/// Data we would be saving for the player.
/// </summary>
[System.Serializable]
public class GameData
{
    [System.Serializable]
    public class ChipData
    {
        public string Name;
        public bool IsUpgraded;
        public int DisableCounter;
    }
    [System.Serializable]
    public class AbilityData
    {

    }
    //Name of Save
    public string SaveName;
    //Level the player is on
    public Levels Level;
    //Player Health
    public int Health;
    //Player MaxHealth;
    public int MaxHealth;
    //PlayerScrap
    public int Scraps;
    // Save Chips
    public List<ChipData> Chips = new List<ChipData>();
    // Save Abilities
    public List<AbilityData> Abilities = new List<AbilityData>();
    public string TimeStampString;
    //time of save
    public DateTime TimeStamp;

    //Default Constructor
    public GameData()
    {
        Chips = new List<ChipData>();
        Abilities= new List<AbilityData>();
    }

    // Synchronize TimeStamp with its string representation
    public void UpdateTimeStamp()
    {
        TimeStampString = TimeStamp.ToString("o"); // Use "o" for ISO 8601 format
    }

    public void ParseTimeStamp()
    {
        if (!string.IsNullOrEmpty(TimeStampString))
            TimeStamp = DateTime.Parse(TimeStampString);
    }
}