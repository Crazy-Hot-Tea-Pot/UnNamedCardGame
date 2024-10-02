using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDefenseChip", menuName = "Chip System/Defense Chip")]
public class DefenseChip : NewChip
{
    /// <summary>
    /// How much shield the card will give.
    /// </summary>
    public int shieldAmount;
    /// <summary>
    /// The amount to upgrade by.
    /// </summary>
    public int upgradedShieldAmountToApply;
    /// <summary>
    /// The buff to apply.
    /// </summary>
    public Effects.Buff buffToApply=Effects.Buff.None;
    /// <summary>
    /// Amount of that buff to apply.
    /// </summary>
    public int buffStacks = 0;
    /// <summary>
    /// Amount to upgrade the buff amount by.
    /// </summary>
    public int upgradedBuffStacksByAmout;
    /// <summary>
    /// List of Debuffs to remove.
    /// Put the main debuff to remove first and the rest will be if upgraded.
    /// </summary>
    [Tooltip("The first debuff is the main Debuff to remove. Any more will be removed if card is upgraded")]
    public List<Effects.Debuff> deBuffsToRemove = new List<Effects.Debuff>();

    public override bool IsUpgraded
    {
        get
        {
            return isUpgraded;
        }
        set
        {
            base.IsUpgraded = value;
            if (isUpgraded)
            {
                shieldAmount += upgradedShieldAmountToApply;
                buffStacks += upgradedBuffStacksByAmout;
            }
        }
    }

    public override void OnChipPlayed(PlayerController player)
    {
        base.OnChipPlayed(player);
        
        //Apply worndown effect
        if (player.IsWornDown)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ApplyShield(Mathf.FloorToInt(shieldAmount*0.7f));
        }
        else
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ApplyShield(shieldAmount);

        //Apply buffs to player
        player.ApplyEffect(buffToApply, buffStacks);
        Debug.Log($"{player.name} receives {buffStacks} stacks of {buffToApply}.");

        // Remove specified debuffs will only remove first one if not upgraded
        foreach (Effects.Debuff debuff in deBuffsToRemove)
        {
            switch (debuff)
            {
                case Effects.Debuff.Gunked:
                    player.GunkStacks--;
                    break;
                case Effects.Debuff.Drained:
                    player.DrainedStacks--;
                    break;
                case Effects.Debuff.Jam:                    
                    player.JammedStacks--;
                    break;
                case Effects.Debuff.WornDown:                    
                    player.WornDownStacks--;
                    break;
                default:
                    Debug.LogWarning($"Debuff {debuff} not recognized.");
                    break;
            }
            Debug.Log($"{player.name} removes the {debuff} debuff.");
            //Break out if not upgraded
            if (!IsUpgraded)
                break;           
        }
    }
}
