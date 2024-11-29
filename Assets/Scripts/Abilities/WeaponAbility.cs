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
            //Cost energy
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().PlayedAbility(energyCost);

            //For each object in the list of targetable objects
            foreach (GameObject enemy in GameObject.Find("Player").GetComponent<PlayerController>().abilityRangedEnemies)
            {
                //Try to deal damage one by one to each enemy
                try
                {
                    //Does damage to the enemy
                    enemy.GetComponent<Enemy>().TakeDamage(damage);
                }
                catch
                {
                    //Assume we couldn't find the enemy
                    Debug.LogWarning("We couldn't find the right enemy object it had no enemy script attached");
                }
            }

    }
}
