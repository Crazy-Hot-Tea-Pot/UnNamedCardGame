using System.Collections.Generic;
using UnityEngine;

public class Level : ScriptableObject
{
    [Header("Level Details")]
    public Levels levelID; // Enum from Levels.cs

    [Header("Enemy Spawns")]
    public List<EnemySpawn> enemySpawns = new();

    [Header("Terminal Settings")]
    [Range(0, 100)]
    // 100 means guaranteed activation
    public int terminalSpawnChance;

    public List<NextLevel> nextLevels = new();
}

[System.Serializable]
public class NextLevel
{
    // The next level to go to
    public Levels levelID;
    // Optional: Quest required to unlock
    public string questCondition;
}
