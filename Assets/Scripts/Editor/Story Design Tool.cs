using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using static GameEnums;

public class StoryDesignTool : EditorWindow
{
    private string storyName = "New Story";
    private StoryPathType pathType = StoryPathType.Linear;
    private Difficulty difficulty = Difficulty.Normal;
    private List<LevelDefinition> levels = new List<LevelDefinition>();
    private Vector2 scrollPos;

    [MenuItem("Tools/Story Designer")]
    public static void OpenWindow()
    {
        GetWindow<StoryDesignTool>("Story Design Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Story Designer", EditorStyles.boldLabel);

        // Story Details Section
        EditorGUILayout.Space();
        GUILayout.Label("Story Details", EditorStyles.boldLabel);
        storyName = EditorGUILayout.TextField("Story Name:", storyName);
        pathType = (StoryPathType)EditorGUILayout.EnumPopup("Path Type:", pathType);
        difficulty = (Difficulty)EditorGUILayout.EnumPopup("Difficulty:", difficulty);

        // Levels Section
        EditorGUILayout.Space();
        GUILayout.Label("Levels", EditorStyles.boldLabel);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(200));
        for (int i = 0; i < levels.Count; i++)
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label($"Level {i + 1}", EditorStyles.boldLabel);

            levels[i].levelID = (Levels)EditorGUILayout.EnumPopup("Level ID:", levels[i].levelID);
            levels[i].terminalSpawnChance = EditorGUILayout.IntSlider("Terminal Spawn Chance:", levels[i].terminalSpawnChance, 0, 100);

            GUILayout.Label("Enemy Spawns", EditorStyles.boldLabel);
            for (int j = 0; j < levels[i].enemySpawns.Count; j++)
            {
                GUILayout.BeginHorizontal();
                levels[i].enemySpawns[j].enemyPrefab = (GameObject)EditorGUILayout.ObjectField("Enemy Prefab:", levels[i].enemySpawns[j].enemyPrefab, typeof(GameObject), false);
                levels[i].enemySpawns[j].count = EditorGUILayout.IntField("Count:", levels[i].enemySpawns[j].count);

                if (GUILayout.Button("Remove", GUILayout.Width(70)))
                {
                    levels[i].enemySpawns.RemoveAt(j);
                    break;
                }
                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Enemy Spawn"))
            {
                levels[i].enemySpawns.Add(new EnemySpawn());
            }

            if (GUILayout.Button("Remove Level"))
            {
                levels.RemoveAt(i);
                break;
            }
            GUILayout.EndVertical();
        }
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Add Level"))
        {
            levels.Add(new LevelDefinition());
        }

        // Save Story Button
        EditorGUILayout.Space();
        if (GUILayout.Button("Save Story"))
        {
            SaveStory();
        }
    }

    private void SaveStory()
    {
        if (string.IsNullOrWhiteSpace(storyName))
        {
            EditorUtility.DisplayDialog("Error", "Story Name cannot be empty!", "OK");
            return;
        }

        if (levels.Count == 0)
        {
            EditorUtility.DisplayDialog("Error", "A story must have at least one level!", "OK");
            return;
        }

        Story newStory = CreateInstance<Story>();
        newStory.storyName = storyName;
        newStory.pathType = pathType;
        newStory.difficulty = difficulty;
        newStory.levels = new List<Level>();

        string folderPath = "Assets/Resources/Scriptables/Stories";
        if (!AssetDatabase.IsValidFolder(folderPath))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Scriptables", "Stories");
        }

        string levelFolderPath = "Assets/Resources/Scriptables/Levels";
        if (!AssetDatabase.IsValidFolder(levelFolderPath))
        {
            AssetDatabase.CreateFolder("Assets/Resources/Scriptables", "Levels");
        }

        foreach (var levelDefinition in levels)
        {
            Level level = CreateInstance<Level>();
            level.levelID = levelDefinition.levelID;
            level.terminalSpawnChance = levelDefinition.terminalSpawnChance;

            // Assign enemy spawns directly without re-wrapping in a new List
            level.enemySpawns = levelDefinition.enemySpawns;            

            string levelPath = $"{levelFolderPath}/{storyName}_Level_{level.levelID}.asset";
            AssetDatabase.CreateAsset(level, levelPath);
            newStory.levels.Add(level);
        }


        string assetPath = $"{folderPath}/{storyName}.asset";
        AssetDatabase.CreateAsset(newStory, assetPath);
        AssetDatabase.SaveAssets();

        EditorUtility.DisplayDialog("Success", "Story and Levels saved successfully!", "OK");

        ResetFields();
    }

    private void ResetFields()
    {
        storyName = "New Story";
        pathType = StoryPathType.Linear;
        difficulty = Difficulty.Normal;
        levels.Clear();
    }
}

public class LevelDefinition
{
    public Levels levelID;
    public int terminalSpawnChance;
    public List<EnemySpawn> enemySpawns = new();
}

[System.Serializable]
public class EnemySpawn
{
    public GameObject enemyPrefab;
    public int count;
}
