using UnityEditor;
using UnityEngine;

public class QuestManagerEditor : EditorWindow
{
    public QuestGoHere GoHereQuest;
    private bool selectionMode = false;

    [MenuItem("Tools/Quest Creator Window")]
    public void OnGUI()
    {
        if (GUILayout.Button("Add Go Here Quest"))
        {
            //Change mouse colour to green
            GUI.skin.settings.cursorColor = Color.green;
            selectionMode = true;
        }
    }
}
