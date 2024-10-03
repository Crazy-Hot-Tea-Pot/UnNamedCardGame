using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : ScriptableObject
{
    /// <summary>
    /// The name of the item
    /// </summary>
    public string itemName;
    /// <summary>
    /// The description of the item
    /// </summary>
    public string itemDescription;


    /// <summary>
    /// This method instantiates the equipment depending on the item
    /// </summary>
    public virtual void ActivateEquipmnet()
    {
        
    }
}
