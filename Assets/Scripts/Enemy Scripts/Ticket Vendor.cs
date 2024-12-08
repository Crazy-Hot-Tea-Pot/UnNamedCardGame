using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
/// <summary>
/// Ticket Vendor: 90 HP (Robot Enemy) SUBWAY EXCLUSIVE
/// Intents:
/// Halt: Deal 9 Damage, Apply 1 Worn & 1 Drained. (30%)
/// Confiscate: Deal 7 Damage and Disable 2 of your Chips. (40%)
/// Redirect: Deal 7 Damage and Disable an Ability for a Turn. (30%)
/// </summary>
public class TicketVendor : Enemy
{
    // Start is called before the first frame update
    public override void Start()
    {
        EnemyName = "Ticket Vendor";
        maxHP = 90;
        base.Start();
    }
    protected override void PerformIntent()
    {
        var tempRandom = Random.Range(1, 11);

        if (tempRandom <= 3)
            Halt();
        else if (tempRandom <= 7)
            Confiscate();
        else
            Redirect();

        base.PerformIntent();
    }
    private void Redirect()
    {
        EnemyTarget.GetComponent<PlayerController>().DamagePlayerBy(7);

        //TODO Sabastian implement ability disabled.
    }
    private void Confiscate()
    {
        EnemyTarget.GetComponent<PlayerController>().DamagePlayerBy(7);

        int temp1 = Random.Range(0, ChipManager.Instance.PlayerHand.Count);

        int temp2 = Random.Range(0, ChipManager.Instance.PlayerHand.Count);

        while (temp1 == temp2)
        {
            temp2 = Random.Range(0, ChipManager.Instance.PlayerHand.Count);
        }

        ChipManager.Instance.PlayerHand[temp1].GetComponent<Chip>().newChip.GetComponent<NewChip>().IsActive = false;
        ChipManager.Instance.PlayerHand[temp2].GetComponent<Chip>().newChip.GetComponent<NewChip>().IsActive = false;
    }
    private void Halt()
    {
        EnemyTarget.GetComponent<PlayerController>().DamagePlayerBy(9);
        EnemyTarget.GetComponent<PlayerController>().ApplyEffect(Effects.Debuff.WornDown, 1);
        EnemyTarget.GetComponent<PlayerController>().ApplyEffect(Effects.Debuff.Drained, 1);
    }
}
