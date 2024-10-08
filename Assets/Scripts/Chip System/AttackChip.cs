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

    public override void OnChipPlayed(PlayerController player)
    {
        base.OnChipPlayed(player);
        
        if (player.IsPowered)
        {
            Debug.Log("Player does Empower attack.");
            GameManager.Instance.enemyList[0].GetComponent<Enemy>().TakeDamage(damage+player.PoweredStacks);
        }
        else if (player.IsDrained)
        {
            GameManager.Instance.enemyList[0].GetComponent<Enemy>().TakeDamage(Mathf.FloorToInt(damage * 0.8f));
            player.DrainedStacks--;
            Debug.Log("Player is Drained! Damage reduced to: " + Mathf.FloorToInt(damage * 0.8f));
        }
        else
        {
            GameManager.Instance.enemyList[0].GetComponent<Enemy>().TakeDamage(damage);
        }

        //Now to apply debuffs
        if (debuffStacks > 0)
        {
            GameManager.Instance.enemyList[0].GetComponent<Enemy>().ApplyDebuff(debuffToApply, debuffStacks);
        }
    }
}
