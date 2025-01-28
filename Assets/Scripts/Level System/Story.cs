using System.Collections.Generic;
using UnityEngine;
using static GameEnums;

public class Story : ScriptableObject
{
    [Header("Story Details")]
    public string storyName;
    public StoryPathType pathType;
    public Difficulty difficulty;

    [Header("Levels")]
    public List<Level> levels = new List<Level>();
}
