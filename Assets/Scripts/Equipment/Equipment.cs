using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "Equipment/New Equipment")]
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
    /// A variable to destroy objects
    /// </summary>
    public bool destroyme = false;


    /// <summary>
    /// This method instantiates the equipment depending on the item
    /// </summary>
    public virtual void ActivateEquipmnet()
    {
        
    }
}
