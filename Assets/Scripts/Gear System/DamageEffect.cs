using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDamageEffect", menuName = "Gear/Effects/DamageEffect")]
public class DamageEffect : ItemEffect
{
    [Header("Weapon values")]
    [Tooltip("Damage dealt")]
    public int baseDamage;

    public override void Activate(PlayerController player, Item item, Enemy enemy)
    {
        int adjustedDamage = baseDamage + item.GetValueIncreaseBy();

        // increase for energy cost
        int adjustedEnergyCost = energyCost + item.GetEnergyCostIncreaseBy();

        if (player.SpendEnergy(adjustedEnergyCost))
        {
            //Play Item Effect
            SoundManager.PlayFXSound(ItemActivate);

            //Do Damage to enemy
            enemy?.TakeDamage(adjustedDamage);

            //Any conditions applied after hit with weapon
            if (SpecialConditionEffect)
            {
                switch (Condition)
                {
                    case ConditionEffect.LessThanHalfHealth:

                        if (enemy.CurrentHP <= (enemy.maxHP / 2))
                        {
                            foreach(Effects.TempDeBuffs debuff in deBuffEffectToApplyToEnemy)
                            {
                                enemy.ApplyDebuff(debuff.DeBuff, debuff.AmountToDeBuff);
                            }
                        }

                    break;
                }
            }

            //Apply any effects to player if any

            foreach(Effects.TempDeBuffs debuff in debuffToApplyToPlayer)
            {
                player.ApplyEffect(debuff.DeBuff, debuff.AmountToDeBuff);
            }

            foreach(Effects.TempBuffs buff in buffToApplyToPlayer)
            {
                player.ApplyEffect(buff.Buff,buff.AmountToBuff);
            }

            if(effectToApplyToPlayer != Effects.Effect.None)                
                player.ApplyEffect(effectToApplyToPlayer);
        }
        else
        {
            SoundManager.PlayFXSound(ItemFail);

            Debug.Log("Not enough energy to use Weapon.");
        }
    }
}