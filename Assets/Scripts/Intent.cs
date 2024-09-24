using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIntent", menuName = "Intent")]
public class Intent : ScriptableObject
{
    /// <summary>
    /// Name of intent
    /// </summary>
    public string intentName;
    /// <summary>
    /// Damage Intent will do.
    /// </summary>
    public int damage;
    /// <summary>
    /// Chance of intent
    /// </summary>
    public int chance;
    /// <summary>
    /// for special effects.
    /// </summary>
    public string effectDescription;

    public virtual void Execute(Enemy enemy)
    {
        // Default behavior: dealing damage
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}
