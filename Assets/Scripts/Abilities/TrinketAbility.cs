﻿using UnityEngine;

[CreateAssetMenu(fileName = "New Trinket Ability", menuName = "Abilities/Trinket")]
public class TrinketAbility : Ability
{
    /// <summary>
    /// how much power gain.
    /// </summary>
    public int powerGain;

    public override void Activate()
    {
        base.Activate();

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().TakeDamage(powerGain);
        //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().RestoreEnergy(powerGain);
    }
}
