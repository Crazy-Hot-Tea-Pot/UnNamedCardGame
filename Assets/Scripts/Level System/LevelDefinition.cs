using System;
using System.Collections.Generic;

[System.Serializable]
public class LevelDefinition
{
    public Levels levelID;
    public int terminalSpawnChance;

    // Enemies now exist directly inside the Story object
    public List<EnemySpawn> enemySpawns = new List<EnemySpawn>();

    // Next levels (for branching and random paths)
    public List<LevelDefinition> nextLevels = new List<LevelDefinition>();

    public Quest questCondition;
}
