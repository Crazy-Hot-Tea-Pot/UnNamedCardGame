using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using static GameEnums;

public class PathGraphWindow : EditorWindow
{
    private List<LevelDefinition> levels;
    private StoryPathType pathType;

    private Vector2 scrollPosition;
    private const float nodeWidth = 150f;
    private const float nodeHeight = 100f;
    private const float horizontalSpacing = 250f;
    private const float verticalSpacing = 150f;

    public static void OpenWindow(List<LevelDefinition> levels, StoryPathType pathType)
    {
        PathGraphWindow window = GetWindow<PathGraphWindow>("Path Graph");
        window.levels = levels;
        window.pathType = pathType;
    }

    private void OnGUI()
    {
        if (levels == null || levels.Count == 0)
        {
            EditorGUILayout.LabelField("No levels to display.");
            return;
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        GUILayout.Label($"Path Graph - {pathType}", EditorStyles.boldLabel);

        Rect canvasRect = new Rect(0, 0, levels.Count * horizontalSpacing, levels.Count * verticalSpacing);
        GUILayout.BeginArea(canvasRect);

        Dictionary<Levels, Vector2> nodePositions = new Dictionary<Levels, Vector2>();

        // Draw nodes and store their positions
        for (int i = 0; i < levels.Count; i++)
        {
            Vector2 position = new Vector2(i * horizontalSpacing, i * verticalSpacing);
            DrawNode(levels[i], position);
            nodePositions[levels[i].levelID] = position;
        }

        GUILayout.EndArea();
        EditorGUILayout.EndScrollView();

        // Draw connections between nodes
        DrawConnections(nodePositions);
    }

    private void DrawNode(LevelDefinition level, Vector2 position)
    {
        Rect nodeRect = new Rect(position.x, position.y, nodeWidth, nodeHeight);
        GUI.Box(nodeRect, level.levelID.ToString());

        GUILayout.BeginArea(nodeRect);
        GUILayout.Label($"Level ID: {level.levelID}");
        GUILayout.Label($"Terminal %: {level.terminalSpawnChance}");
        GUILayout.EndArea();
    }

    private void DrawConnections(Dictionary<Levels, Vector2> nodePositions)
    {
        Handles.BeginGUI();

        foreach (var level in levels)
        {
            Vector2 fromPosition = nodePositions[level.levelID] + new Vector2(nodeWidth / 2, nodeHeight);

            foreach (var next in level.nextLevels)
            {
                if (nodePositions.TryGetValue(next.levelID, out Vector2 toPosition))
                {
                    toPosition += new Vector2(nodeWidth / 2, 0); // Adjust for center alignment
                    DrawConnectionLine(fromPosition, toPosition, next.questCondition);
                }
            }
        }

        Handles.EndGUI();
    }

    private void DrawConnectionLine(Vector2 from, Vector2 to, string questCondition)
    {
        // Draw connection line
        Handles.DrawBezier(from, to, from + Vector2.down * 50, to + Vector2.up * 50, Color.white, null, 2f);

        // Draw quest condition label (for conditional paths)
        if (!string.IsNullOrEmpty(questCondition))
        {
            Vector2 midPoint = (from + to) / 2;
            GUI.Label(new Rect(midPoint.x, midPoint.y, 100, 20), questCondition, EditorStyles.boldLabel);
        }
    }
}
