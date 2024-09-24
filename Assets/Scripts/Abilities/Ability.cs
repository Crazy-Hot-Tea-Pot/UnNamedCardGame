using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    /// <summary>
    /// Name of Ability
    /// </summary>
    public string abilityName;

    /// <summary>
    /// Description of Ability;
    /// </summary>
    public string abilityDescription;

    /// <summary>
    /// Cost to do ability
    /// </summary>
    public int energyCost;

    /// <summary>
    /// Activate Ability
    /// </summary>
    public virtual void Activate()
    {

    }
    
}
