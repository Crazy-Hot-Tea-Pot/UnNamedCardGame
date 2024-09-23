using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Ability", menuName = "Abilities/Weapon")]
public class WeaponAbility : Ability
{
    /// <summary>
    /// Amout of Damage this ability will do
    /// </summary>
    public int damage;

    public override void Activate()
    {

    }
}
