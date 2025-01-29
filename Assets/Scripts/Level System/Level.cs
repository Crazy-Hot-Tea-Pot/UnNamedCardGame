using System.Collections.Generic;
using UnityEngine;

public class Level : ScriptableObject
{
    [Header("Level Details")]
    // Enum from Levels.cs
    public Levels levelName;

    [Header("Enemy Spawns")]
    public List<EnemySpawn> enemySpawns = new();

    [Header("Terminal Settings")]
    [Range(0, 100)]
    // 100 means guaranteed activation
    public int terminalSpawnChance;

    //TODO limit this to max 2 levels
    public List<Level> nextLevelInBranch = new();

    //Optional: Quest required to unlock
    public Quest questCondition;
}