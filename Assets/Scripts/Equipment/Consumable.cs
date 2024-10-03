using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Equipment
{
    /// <summary>
    /// How much health this consumable restores
    /// </summary>
    public int restoreHP;
    /// <summary>
    /// How much energy this consumable restores
    /// </summary>
    public int restoreEnergy;
    /// <summary>
    /// How much shield this consumable restores
    /// </summary>
    public int restoreShield;

    public override void ActivateEquipmnet()
    {
        //Does the method base first
        base.ActivateEquipmnet();

        //Find the player controller and in order add shield, add health and add energy based on our variables above
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ApplyShieldToPlayer(restoreShield);
        
    }
}
