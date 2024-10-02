using UnityEngine;

[CreateAssetMenu(fileName = "NewKickStarterEffect", menuName = "Chip System/Skill Effects/KickStarter")]
public class KickStarterEffect : SkillEffects
{
    public override void ApplyEffect(PlayerController player)
    {
        Debug.Log("KickStarter Effect: Recovered " + (Mathf.FloorToInt(player.Energy * 0.5f)) + " energy.");
        player.RecoverEnergy((Mathf.FloorToInt(player.Energy * 0.5f)));        
    }
}
