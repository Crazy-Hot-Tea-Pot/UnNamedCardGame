using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAction : ScriptableObject
{
    public string abilityName;

    public virtual void CardActionActivate()
    {
        Debug.Log("Card in use.");
    }

}
