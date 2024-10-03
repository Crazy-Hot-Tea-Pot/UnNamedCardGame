using UnityEngine;
[CreateAssetMenu(fileName = "NewBuffEffect", menuName = "Chip System/Skill Effects/Buff")]
public class Buff : SkillEffects
{

    public Effects.Buff buffToApply;

    public int amountOfBuffToApply;
    public int amountOfbuffToUpgradeBy;

    protected override void ChipUpgraded()
    {
        base.ChipUpgraded();
        if(IsUpgraded)
        {
            if (isUpgraded)
                amountOfBuffToApply += amountOfbuffToUpgradeBy;
        }
        else
            amountOfBuffToApply -= amountOfbuffToUpgradeBy;
    }
    public override void ApplyEffect(PlayerController player)
    {
        player.ApplyEffect(buffToApply, amountOfBuffToApply);
    }
}
