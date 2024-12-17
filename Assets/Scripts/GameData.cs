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
        /// <summary>
        /// Name of Chip
        /// </summary>
        public string Name;
        /// <summary>
        /// If Chip is Upgraded
        /// </summary>
        public bool IsUpgraded;
        /// <summary>
        /// count for turns chip disable.
        /// </summary>
        public int DisableCounter;
    }
    [System.Serializable]
    public class GearData
    {
        public string GearName;        
        public bool IsEquipped;
        public bool IsPlayerOwned;

        /// <summary>
        /// Item Teir Currently
        /// </summary>
        public string Teir;
    }

    //Name of Save
    public string SaveName;
    //time of save
    public DateTime TimeStamp;
    //Level the player is on
    public Levels Level;
    //Player HealthBar
    public int Health;
    //Player MaxHealth;
    public int MaxHealth;
    //PlayerScrap
    public int Scraps;
    // Save Chips
    public List<ChipData> Chips = new List<ChipData>();
    // Save Gears
    public List<GearData> Gear = new List<GearData>();
    public string TimeStampString;

    //Default Constructor
    public GameData()
    {
        Chips = new List<ChipData>();
        Gear = new List<GearData>();
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