using UnityEngine;

[CreateAssetMenu(fileName = "NewArmorEffect", menuName = "Gear/Effects/ArmorEffect")]
public class ArmorEffect : ItemEffect
{
    [Header("Armor values")]
    [Tooltip("Shield Amount")]
    public int baseShieldAmount;

    public override void Activate(PlayerController player, Item item, Enemy enemy = null)
    {
        int adjustedShieldAmount = baseShieldAmount + item.GetValueIncreaseBy();

        // increase for energy cost
        int adjustedEnergyCost = energyCost + item.GetEnergyCostIncreaseBy();

        if (player.SpendEnergy(adjustedEnergyCost))
        {
            //Play Item Effect
            SoundManager.PlayFXSound(ItemActivate);

            player.ApplyShield(adjustedShieldAmount);

            foreach (Effects.TempBuffs buff in buffToApplyToPlayer)
            {
                player.ApplyEffect(buff.Buff, buff.AmountToBuff);
            }
            if (effectToApplyToPlayer != Effects.Effect.None)
                player.ApplyEffect(effectToApplyToPlayer);

        }
        else
        {
            SoundManager.PlayFXSound(ItemFail);

            Debug.Log("Not enough energy to use Armor.");
        }
    }
}
