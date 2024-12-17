using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    private SerializedProperty valueIncreaseBy;
    private SerializedProperty energyCostIncreaseBy;

    private void OnEnable()
    {
        // Cache the serialized properties
        valueIncreaseBy = serializedObject.FindProperty("valueIncreaseBy");
        energyCostIncreaseBy = serializedObject.FindProperty("energyCostIncreaseBy");
    }

    public override void OnInspectorGUI()
    {
        // Update the serialized object
        serializedObject.Update();

        // Draw default inspector for other fields
        DrawPropertiesExcluding(serializedObject, "valueIncreaseBy", "energyCostIncreaseBy");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Tier values to increase by", EditorStyles.boldLabel);

        // Display the Tier Values as the enum names
        DrawTierList("Damage/Shield Increase", valueIncreaseBy);
        DrawTierList("Energy Cost Increase", energyCostIncreaseBy);

        // Apply changes
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawTierList(string label, SerializedProperty list)
    {
        EditorGUILayout.LabelField(label, EditorStyles.miniBoldLabel);

        string[] tierNames = System.Enum.GetNames(typeof(Item.Teir));

        for (int i = 0; i < tierNames.Length; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(tierNames[i], GUILayout.Width(100));
            list.GetArrayElementAtIndex(i).intValue = EditorGUILayout.IntField(list.GetArrayElementAtIndex(i).intValue);
            EditorGUILayout.EndHorizontal();
        }
    }
}
