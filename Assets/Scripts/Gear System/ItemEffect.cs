using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemEffect : ScriptableObject
{

    public bool IsEquipped
    {
        get
        {
            return isEquipped;
        }
        set
        {
            isEquipped = value;

            Equipped();
        }
    }

    public enum ConditionEffect { LessThanHalfHealth }

    public string ItemEffectDescription;
    [Tooltip("Cost to use the ItemEffect")]
    public int energyCost;

    [Header("Sound Effects")]
    public SoundFX ItemActivate;
    public SoundFX ItemDeactivate;
    public SoundFX ItemFail;

    [Space(20)]

    [Tooltip("If effect is only applied on special conditions.")]
    public bool SpecialConditionEffect = false;
    public ConditionEffect Condition;
    public List<Effects.TempDeBuffs> deBuffEffectToApplyToEnemy = new();


    [Header("Effects if applicable to apply to player on Use")]
    public List<Effects.TempBuffs> buffToApplyToPlayer = new();
    public List<Effects.TempDeBuffs> debuffToApplyToPlayer = new();
    public Effects.Effect effectToApplyToPlayer;

    private bool isEquipped = false;

    public virtual void Activate(PlayerController player, Item item, Enemy enemy = null)
    {

    }

    protected virtual void Equipped()
    {

    }
}
