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
    private List<GameObject> enemyPrefabs = new List<GameObject>();
    private string[] enemyNames;


    [MenuItem("Tools/Story Designer")]
    public static void OpenWindow()
    {
        GetWindow<StoryDesignTool>("Story Design Tool");
    }

    void OnEnable()
    {
        LoadEnemyPrefabs();
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
        try
        {
            for (int i = 0; i < levels.Count; i++)
            {
                GUILayout.BeginVertical("box");
                try
                {
                    GUILayout.Label($"Level {i + 1}", EditorStyles.boldLabel);

                    levels[i].levelID = (Levels)EditorGUILayout.EnumPopup("Level ID:", levels[i].levelID);
                    levels[i].terminalSpawnChance = EditorGUILayout.IntSlider("Terminal Spawn Chance by %", levels[i].terminalSpawnChance, 0, 100);

                    GUILayout.Label("Enemy Spawns", EditorStyles.boldLabel);
                    for (int j = 0; j < levels[i].enemySpawns.Count; j++)
                    {
                        GUILayout.BeginHorizontal();
                        try
                        {
                            // Dropdown for selecting enemy prefab
                            int selectedIndex = Mathf.Max(0, enemyPrefabs.IndexOf(levels[i].enemySpawns[j].enemyPrefab));
                            selectedIndex = EditorGUILayout.Popup("Enemy Type:", selectedIndex, enemyNames, GUILayout.Width(200));
                            levels[i].enemySpawns[j].enemyPrefab = enemyPrefabs[selectedIndex];

                            // Enemy name
                            levels[i].enemySpawns[j].enemyName = EditorGUILayout.TextField("Enemy Name:", levels[i].enemySpawns[j].enemyName);

                            if (GUILayout.Button("Remove", GUILayout.Width(70)))
                            {
                                levels[i].enemySpawns.RemoveAt(j);
                                break;
                            }
                        }
                        finally
                        {
                            GUILayout.EndHorizontal();
                        }
                    }

                    if (GUILayout.Button("Add Enemy Spawn"))
                    {
                        levels[i].enemySpawns.Add(new EnemySpawn());
                    }

                    if (pathType != StoryPathType.Linear)
                    {
                        GUILayout.Label("Next Levels", EditorStyles.boldLabel);
                        for (int k = 0; k < levels[i].nextLevels.Count; k++)
                        {
                            GUILayout.BeginHorizontal();
                            try
                            {
                                levels[i].nextLevels[k].levelID = (Levels)EditorGUILayout.EnumPopup($"Next Level {k + 1}:", levels[i].nextLevels[k].levelID);
                                if (pathType == StoryPathType.Conditional)
                                {
                                    levels[i].nextLevels[k].questCondition = EditorGUILayout.TextField("Quest Condition:", levels[i].nextLevels[k].questCondition);
                                }
                                if (GUILayout.Button("Remove", GUILayout.Width(70)))
                                {
                                    levels[i].nextLevels.RemoveAt(k);
                                    break;
                                }
                            }
                            finally
                            {
                                GUILayout.EndHorizontal();
                            }
                        }
                        if (GUILayout.Button("Add Next Level"))
                        {
                            levels[i].nextLevels.Add(new NextLevel());
                        }
                    }

                    if (GUILayout.Button("Remove Level"))
                    {
                        levels.RemoveAt(i);
                        break;
                    }
                }
                finally
                {
                    GUILayout.EndVertical();
                }
            }
        }
        finally
        {
            EditorGUILayout.EndScrollView();
        }

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

        // Graph Visualization Button
        if (GUILayout.Button("Visualize Path Graph"))
        {
            PathGraphWindow.OpenWindow(levels, pathType);
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

        // Validate enemy names
        if (!ValidateEnemyNames())
        {
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

            // Save enemy spawns with names
            level.enemySpawns = new List<EnemySpawn>();
            foreach (var spawn in levelDefinition.enemySpawns)
            {
                level.enemySpawns.Add(new EnemySpawn
                {
                    enemyPrefab = spawn.enemyPrefab,
                    enemyName = spawn.enemyName
                });
            }

            level.nextLevels = new List<NextLevel>(levelDefinition.nextLevels);

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
    private bool ValidateEnemyNames()
    {
        HashSet<string> names = new HashSet<string>();
        foreach (var level in levels)
        {
            foreach (var enemy in level.enemySpawns)
            {
                if (!names.Add(enemy.enemyName))
                {
                    EditorUtility.DisplayDialog("Error", $"Duplicate enemy name detected: {enemy.enemyName}", "OK");
                    return false;
                }
            }
        }
        return true;
    }

    private void LoadEnemyPrefabs()
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets/Prefabs/Enemies" });
        enemyPrefabs.Clear();
        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            
            if(prefab.tag=="Enemy")
                if (prefab != null) 
                    enemyPrefabs.Add(prefab);
        }
        enemyNames = enemyPrefabs.ConvertAll(p => p.name).ToArray();
    }

}