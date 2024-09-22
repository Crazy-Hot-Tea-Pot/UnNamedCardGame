using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardAction", menuName = "Card Action")]
public class CardAction : ScriptableObject
{
    /// <summary>
    /// The name of the action.
    /// </summary>
    public string abilityName;
    /// <summary>
    /// A description of what the ability does.
    /// </summary>
    public string description;
    /// <summary>
    /// A visual icon for the ability.
    /// </summary>
    public Sprite abilityIcon;

    public virtual void CardActionActivate()
    {
        Debug.Log("Card in use.");
    }

}
