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
    /// <summary>
    /// TODO go over your structure think this can be improved
    /// </summary>
    /// <param name="player"></param>
    public override void OnChipPlayed(PlayerController player)
    {
        base.OnChipPlayed(player);

        // If the effect is player-only
        if (specialEffect != null)
        {
            if (specialEffect is LeechEffect leechEffect)
            {
                // Get target enemy for Leech effect
                Enemy target = GameManager.Instance.enemyList[0].GetComponent<Enemy>();
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
 }
