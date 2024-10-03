using UnityEditor;
using UnityEngine;

/// <summary>
/// This is an editor for the card script to allow creating of cards easier in the future.
/// </summary>
[CustomEditor(typeof(Chip))]
public class CardScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get the reference to the Chip component
        //Chip card = (Chip)target;

        base.OnInspectorGUI();

        // Display fields in the desired order
        //EditorGUILayout.LabelField("Is Active", card.IsActive.ToString());
        //card.ChipTitle = EditorGUILayout.TextField("Chip Title", card.ChipTitle);
        //card.CardDescription = EditorGUILayout.TextField("Chip Description", card.CardDescription);
        //card.Uses = EditorGUILayout.IntField("Uses", card.Uses);
        //// Display the Chip Image field as a Sprite
        //card.CardImage = (Sprite)EditorGUILayout.ObjectField("Chip Image", card.CardImage, typeof(Sprite), false);

        // Display the Chip Ability dropdown
        //card.CardAbility = (Chip.CardAbilities)EditorGUILayout.EnumPopup("Chip Ability", card.CardAbility);

        //// Display additional fields if the CardAbility is set to Weapon
        //switch (card.CardAbility)
        //{
        //    case Chip.CardAbilities.NoAbility:
        //        // No Fields
        //        break;
        //    case Chip.CardAbilities.Weapon:
        //        card.energyCost = EditorGUILayout.IntField("Energy", card.energyCost);
        //        card.damageAmount = EditorGUILayout.IntField("Damage", card.damageAmount);
        //        break;
        //    case Chip.CardAbilities.Armor:
        //        card.energyCost = EditorGUILayout.IntField("Energy", card.energyCost);
        //        card.sheildAmount = EditorGUILayout.IntField("Shield", card.sheildAmount);
        //        break;
        //    case Chip.CardAbilities.Trinket:
        //        card.energyCost = EditorGUILayout.IntField("Energy", card.energyCost);
        //        card.powerAmount = EditorGUILayout.IntField("Power",card.powerAmount);
        //        break;
        //    default:
        //        break;
        //}

        // Apply changes to the serialized object
        //if (GUI.changed)
        //{
        //    EditorUtility.SetDirty(card);
        //}
    }
}
