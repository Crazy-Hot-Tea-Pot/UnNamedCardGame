using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Ability", menuName = "Abilities/Attack")]
public class AttackAbility : Ability
{
    /// <summary>
    /// Amout of Damage this ability will do
    /// </summary>
    public int damage;

    public override void Activate()
    {

    }
}
