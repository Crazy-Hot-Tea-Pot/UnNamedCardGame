using UnityEditor;
using UnityEngine;

/// <summary>
/// This is an editor for the card script to allow creating of cards easier in the future.
/// </summary>
[CustomEditor(typeof(Card))]
public class CardScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get the reference to the Card component
        //Card card = (Card)target;

        base.OnInspectorGUI();

        // Display fields in the desired order
        //EditorGUILayout.LabelField("Is Active", card.IsActive.ToString());
        //card.CardTitle = EditorGUILayout.TextField("Card Title", card.CardTitle);
        //card.CardDescription = EditorGUILayout.TextField("Card Description", card.CardDescription);
        //card.Uses = EditorGUILayout.IntField("Uses", card.Uses);
        //// Display the Card Image field as a Sprite
        //card.CardImage = (Sprite)EditorGUILayout.ObjectField("Card Image", card.CardImage, typeof(Sprite), false);

        // Display the Card Ability dropdown
        //card.CardAbility = (Card.CardAbilities)EditorGUILayout.EnumPopup("Card Ability", card.CardAbility);

        //// Display additional fields if the CardAbility is set to Weapon
        //switch (card.CardAbility)
        //{
        //    case Card.CardAbilities.NoAbility:
        //        // No Fields
        //        break;
        //    case Card.CardAbilities.Weapon:
        //        card.energyCost = EditorGUILayout.IntField("Energy", card.energyCost);
        //        card.damageAmount = EditorGUILayout.IntField("Damage", card.damageAmount);
        //        break;
        //    case Card.CardAbilities.Armor:
        //        card.energyCost = EditorGUILayout.IntField("Energy", card.energyCost);
        //        card.sheildAmount = EditorGUILayout.IntField("Shield", card.sheildAmount);
        //        break;
        //    case Card.CardAbilities.Trinket:
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
