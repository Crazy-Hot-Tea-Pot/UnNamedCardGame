using System.Collections.Generic;

[System.Serializable]
public class LevelDefinition
{
    public Levels levelID;
    public int terminalSpawnChance;
    public List<EnemySpawn> enemySpawns = new List<EnemySpawn>();
    public List<NextLevel> nextLevels = new List<NextLevel>();
}
