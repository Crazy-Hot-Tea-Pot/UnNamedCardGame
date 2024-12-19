using UnityEngine;

[CreateAssetMenu(fileName = "NewSkillChip", menuName = "Chip System/Skill Chip")]
public class SkillChip : NewChip
{
    public SkillEffects specialEffect;

    public override bool IsUpgraded { 
        get => base.IsUpgraded;
        set { 
            base.IsUpgraded = value;
            specialEffect.IsUpgraded = value;
        } 
    }

    public override void OnChipPlayed(PlayerController player, Enemy target)
    {
        base.OnChipPlayed(player,target);

        // If the effect is player-only. might change to switch case later
        if (specialEffect != null)
        {
            if (specialEffect is LeechEffect leechEffect)
            {
                // Get target enemy for Leech effect                
                leechEffect.ApplyEffect(player, target,this);
            }
            else
            {
                // Apply player-only effect
                specialEffect.ApplyEffect(player);
            }
            Debug.Log(chipName + " triggered a special effect: " + specialEffect.name);
        }
        else
        {
            Debug.LogWarning("No special effect assigned to this card.");
        }
    }

    void OnValidate()
    {
        ChipType = TypeOfChips.Skill;
        Debug.Log($"{name}: ChipType set to {ChipType}");
    }

}
