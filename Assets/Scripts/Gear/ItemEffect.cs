using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemEffect", menuName = "Gear/ItemEffect")]
public class ItemEffect : ScriptableObject
{
    /// <summary>
    /// If this effect is passive
    /// </summary>
    public bool IsPassiveEffect;

    public string ItemEffectDescription;
    [Tooltip("Cost to use the ItemEffect")]
    public int energyCost;
    [Tooltip("Damage dealt")]
    public int damage;
    [Tooltip("Shield Amount")]
    public int shield;
    [Tooltip("Amount of Scrap")]
    public int scrapAmount;

    [Tooltip("If effect is only applied on special conditions.")]
    public bool SpecialConditionEffect = false;

    public bool WhenTargetHasLessThanHalfHealth=false;

    public List<Effects.TempBuffs> buffToApply = new();
    public List<Effects.TempDeBuffs> debuffToApply = new();
    public Effects.Effect effectToApply = new();

    public void Activate()
    {
        Debug.Log($"Activated {ItemEffectDescription}");
       
    }
}
