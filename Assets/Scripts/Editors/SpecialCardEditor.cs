using UnityEditor;
using UnityEngine;
using static SkillChip;

[CustomEditor(typeof(SkillChip))]
public class SpecialCardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //SkillChip specialCard = (SkillChip)target;

        //// Draw the default inspector fields (card name, rarity, description, etc.)
        DrawDefaultInspector();

        //// Draw the fields manually to control visibility
        ////EditorGUILayout.LabelField("Is Upgraded", specialCard.IsUpgraded.ToString());
        ////specialCard.chipName = EditorGUILayout.TextField("Chip Name", specialCard.chipName);
        ////specialCard.description = EditorGUILayout.TextField("Description", specialCard.description);
        ////specialCard.chipRarity = (NewChip.ChipRarity)EditorGUILayout.EnumPopup("Chip Rarity", specialCard.chipRarity);
        ////specialCard.specialEffect = (SkillChip.SkillEffects)EditorGUILayout.EnumPopup("Special Effect", specialCard.specialEffect);
        ////specialCard.canBeUpgraded = EditorGUILayout.Toggle("Can Be Upgraded", specialCard.canBeUpgraded);

        ////// Only show the damageAmount field if the selected effect is Leech
        //////if (specialCard.specialEffect == SkillChip.SkillEffects.Leech)
        //////{
        ////switch (specialCard.specialEffect)
        ////{
        ////    case SkillEffects.Leech:
        ////        specialCard.damageAmount = EditorGUILayout.IntField("Damage Amount", specialCard.damageAmount);
        ////        break;
        ////    default:
        ////        break;
        ////}            
        //// }

        ////Decided to do it in reverse for mental state MIGHT NOT KEEP AND STILL EXPERIMENTING
        //// Start serializing the object so we can exclude certain fields
        //serializedObject.Update();

        //// Draw all fields except damageAmount
        //SerializedProperty prop = serializedObject.GetIterator();
        //prop.NextVisible(true); // Move to the first property

        //// Draw all properties except damageAmount
        //while (prop.NextVisible(false))
        //{
        //    if (prop.name != "damageAmount")
        //    {
        //        EditorGUILayout.PropertyField(prop, true);
        //    }
        //}

        //// Conditionally hide the 'damageAmount' field unless the selected effect is Leech
        //switch (specialCard.specialEffect)
        //{
        //    case SkillEffects.Leech:
        //        specialCard.damageAmount = EditorGUILayout.IntField("Damage Amount", specialCard.damageAmount);
        //        break;
        //    default:
        //        EditorGUILayout.HelpBox("Unknown or Basic effect. No specific fields to display.", MessageType.Warning);
        //        break;
        //}

        //// Apply changes made to serialized properties
        //serializedObject.ApplyModifiedProperties();

        //// Save any changes made to the ScriptableObject
        //if (GUI.changed)
        //{
        //    EditorUtility.SetDirty(specialCard);
        //}
    }
}
