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
    public class ItemData
    {
        public string GearName;
        public int AmountOfAbilities;
        public bool isEquipped;
        public List<AbilityData> ListOfAbilities= new List<AbilityData>();
    }
    [System.Serializable]
    public class AbilityData
    {
        public string AbilityName;
        public bool IsUpgraded;
    }
    //Name of Save
    public string SaveName;
    //time of save
    public DateTime TimeStamp;
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
    // Save Gears
    public List<ItemData> Gears = new List<ItemData>();
    public string TimeStampString;

    //Default Constructor
    public GameData()
    {
        Chips = new List<ChipData>();
        Gears= new List<ItemData>();
    }

    // Synchronize TimeStamp with its string representation
    public void UpdateTimeStamp()
    {
        TimeStampString = TimeStamp.ToString("yyyy-MM-dd HH:mm");
    }

    public void ParseTimeStamp()
    {
        if (!string.IsNullOrEmpty(TimeStampString))
            TimeStamp = DateTime.Parse(TimeStampString);
    }
}