using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageBot : Enemy
{
    public override void Start()
    {
        CurrentHP = maxHP;
        base.Start();
    }
    protected override void PerformIntent()
    {
        int tempRange = Random.Range(1, 11);

        if (tempRange <= 3)
            Compact();
        else if (tempRange <= 6)
            Shred();
        else
            PileOn();

        base.PerformIntent();
    }
    /// <summary>
    /// Deals 15 damage.
    /// Has 30% chance
    /// </summary>
    private void Compact()
    {
        Debug.Log("Garbage Bot uses Compact!");
        EnemyTarget.GetComponent<PlayerController>().DamagePlayerBy(15);        
    }
    /// <summary>
    /// Deal 10 Damage
    /// Apply 1 jam
    /// 50% chance
    /// </summary>
    private void PileOn()
    {
        Debug.Log("Garbage Bot uses Pile On!");        

        EnemyTarget.GetComponent<PlayerController>().DamagePlayerBy(10);
        EnemyTarget.GetComponent<PlayerController>().AddEffect(Effects.Debuff.Jam, 1);
    }
    /// <summary>
    /// Gain 7 shield
    /// Deal 7 damage
    /// 30% chance
    /// </summary>
    private void Shred()
    {
        //Play Sound
        SoundManager.PlayFXSound(SoundFX.ShredGarbageBot,this.gameObject.transform);

        ApplyShield(7);
        EnemyTarget.GetComponent<PlayerController>().DamagePlayerBy(7);
    }
}
