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
        //Decrease the players energy by the appropriate amount
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PlayedCardOrAbility(energyCost);        
    }

    /// <summary>
    /// A method activated on update for passive abilites. Rules about how often and when it activates should be over written on this method
    /// </summary>
    public virtual void PassiveAbility()
    {
        //Designed to be overwritten
    }
    
}
