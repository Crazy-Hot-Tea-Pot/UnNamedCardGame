using UnityEngine;
using static Effects;

[CreateAssetMenu(fileName = "NewEquipmentEffect", menuName = "Gear/Effects/EquipmentEffect")]
public class EquipmentEffect : ItemEffect
{
    [Header("Equipment values")]
    public string SpecialEffectName;    

    /// <summary>
    /// If this effect is passive
    /// </summary>
    public bool HasPassiveEffect;

    public Effects.Effect passiveEffect;

    public bool IsPassiveEffectActive
    {
        get
        {
            return HasPassiveEffect;
        }
        set
        {
            HasPassiveEffect = value;
        }
    }

    private bool isPassiveEffectActive = false;

    public override void Activate(PlayerController player, Item item, Enemy enemy = null)
    {
        if (player.SpendEnergy(energyCost))
        {
            //Play Item Effect
            SoundManager.PlayFXSound(ItemActivate);

            player.ApplyEffect(effectToApplyToPlayer);
        }
        else
        {
            SoundManager.PlayFXSound(ItemFail);

            Debug.Log("Not enough energy to use Equipment.");
        }
    }
    protected override void Equipped()
    {
        base.Equipped();

        if (HasPassiveEffect)
        {
            if (IsEquipped)
            {
                if (!IsPassiveEffectActive)
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().ApplyEffect(passiveEffect);
                    IsPassiveEffectActive = true;
                }
                else
                {
                    GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().RemoveEffect(passiveEffect);
                }
            }            
        }
    }
}
