using UnityEngine;

[CreateAssetMenu(fileName = "NewMotivateEffect", menuName = "Chip System/Skill Effects/Motivate")]
public class MotivateEffect : SkillEffects
{
    public override void ApplyEffect(PlayerController player)
    {
        player.ApplyEffect(Effects.Effect.Motivation);
        Debug.Log("Motivate Effect: Next chip activates twice.");
    }
}
