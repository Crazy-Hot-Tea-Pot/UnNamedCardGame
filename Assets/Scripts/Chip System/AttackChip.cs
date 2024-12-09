using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackChip", menuName = "Chip System/Attack Chip")]
public class AttackChip : NewChip
{
    /// <summary>
    /// Amount of Damage Chip will do.
    /// </summary>
    public int damage;
    /// <summary>
    /// Amount of Damage to do when upgraded.
    /// </summary>
    public int upgradedDamageByAmount;    
    public int numberOfHits=1;
    public Effects.Debuff debuffToApply;
    public int debuffStacks = 0;
    public int upgradedDebuffStacksByAmout;

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
                damage += upgradedDamageByAmount;
                debuffStacks += upgradedDebuffStacksByAmout;
            }
        }
    }

    public override void OnChipPlayed(PlayerController player, Enemy Target)
    {
        base.OnChipPlayed(player,Target);
        
        if (player.IsPowered)
        {
            Debug.Log("Player does Empower attack.");
            Target.TakeDamage(damage+player.PoweredStacks);
        }
        else if (player.IsDrained)
        {
            Target.TakeDamage(Mathf.FloorToInt(damage * 0.8f));
            player.RemoveEffect(Effects.Debuff.Drained, 1,false);            
            Debug.Log("Player is Drained! Damage reduced to: " + Mathf.FloorToInt(damage * 0.8f));
        }
        else
        {
            Target.TakeDamage(damage);
        }

        //Now to apply debuffs
        if (debuffStacks > 0)
        {
            Target.ApplyDebuff(debuffToApply, debuffStacks);
        }
    }

    void OnValidate()
    {
        ChipType = TypeOfChips.Attack;
    }
}
