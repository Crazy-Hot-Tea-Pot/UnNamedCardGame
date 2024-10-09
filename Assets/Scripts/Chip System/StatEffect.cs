using UnityEngine;

[CreateAssetMenu(fileName = "NewStatEffect", menuName = "Chip System/Skill Effects/Stat Effect")]
/// <summary>
/// This scriptable is for effecting player Stats
/// </summary>
public class StatEffect : SkillEffects
{
    // Add more stats as needed
    public enum StatType
    {
        Health,
        Energy,
        Shield 
    }
    public StatType statWillBeEffect;
    // Fill Stat to max.
    public bool toFull;
    public bool toHalf;
    public int amount;
    public int upgradedAmount;

    protected override void ChipUpgraded()
    {
        if (isUpgraded)
        {
            amount += upgradedAmount;
        }
        else
        {
            amount -= upgradedAmount;
        }
    }
    // Apply the effect (modifies player stats based on the specified stat type)
    public override void ApplyEffect(PlayerController player)
    {
        switch (statWillBeEffect)
        {
            case StatType.Health:
                if (toFull)
                    player.FullHeal();
                else
                    player.Heal(amount);
                Debug.Log("Player's health modified by: " + amount);
                break;
            case StatType.Energy:
                if (toFull)
                    player.RecoverFullEnergy();
                else if(toHalf)
                    player.RecoverEnergy((Mathf.FloorToInt(player.Energy * 0.5f)));
                else
                    player.RecoverEnergy(amount);
                Debug.Log("Player's energy modified by: " + amount);
                break;
            case StatType.Shield:
                player.ApplyShield(amount);
                Debug.Log("Player's shield modified by: " + amount);
                break;
            default:
                Debug.LogWarning("Unknown stat type: " + statWillBeEffect);
                break;
        }
    }
}

