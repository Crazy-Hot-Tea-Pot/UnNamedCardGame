using System.Collections.Generic;

[System.Serializable]
public class LevelDefinition
{
    public Levels LevelName;
    /// <summary>
    /// Chance for terminal to spawn.
    /// </summary>
    public int terminalSpawnChance;
    public List<EnemySpawn> enemySpawns = new List<EnemySpawn>();
    public List<Level> nextLevelInBranch = new List<Level>();
}
