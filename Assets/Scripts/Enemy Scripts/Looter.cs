using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looter : Enemy
{
    // To keep track of stolen scrap
    [SerializeField]
    private int stolenScrap = 0;

    // To track the number of Swipes performed
    [SerializeField]
    private int swipeCount = 0;

    private bool shrouded;

    public bool Shrouded
    {
        get { return shrouded; }
        private set { shrouded = value; }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        EnemyName = "Looter";
        maxHP = 60;
        swipeCount = 0;
        stolenScrap = 0;

        base.Start();
    }

    public override void Initialize()
    {       
        base.Initialize();
        Drop weaponShiv = new("Shiv",Drop.DropType.Weapon, 5, 2);
        Drop scrap = new(Drop.DropType.Scrap, 15);//new(Drop.DropType.Scrap, 15);
        enemyDrops.Add(weaponShiv);
        enemyDrops.Add(scrap);
    }

    public override void PerformIntent()
    {       
        // Since 100% on first chance i just made it this way.

        if (swipeCount < 3) // First three turns are Swipe
        {
            Swipe();
        }
        else if (swipeCount == 3 && !Shrouded) // After three Swipes, do Shroud
        {
            Shroud();
        }
        else if (Shrouded) // After Shroud, perform Escape
        {
            Escape();
        }

        base.PerformIntent();
    }
    /// <summary>
    /// 
    ///  After the 3rd Swipe, perform Shroud
    /// </summary>
    private void Swipe()
    {
        Debug.Log($"{EnemyName} performs Swipe, dealing 4 damage and stealing 5 Scrap.");
        swipeCount++;

        stolenScrap += GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().StealScrap(5);

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().TakeDamage(6);

    }

    private void Shroud()
    {
        Debug.Log($"{EnemyName} performs Shroud, gaining 10 Shield.");
        Shield += 10;

        Shrouded = true;
    }

    private void Escape()
    {
        Debug.Log($"{EnemyName} performs Escape, exiting the fight with {stolenScrap} Scrap.");
        CombatController.RemoveCombadant(this.gameObject);
        Destroy( this.gameObject );
    }
    /// <summary>
    /// Return all stolen scrap upon killing
    /// </summary>
    public override void Die()
    {        
        ReturnStolenScrap();
        base.Die();
    }

    private void ReturnStolenScrap()
    {
        Debug.Log($"{EnemyName} returns {stolenScrap} Scrap upon defeat.");
        // Logic to add stolen scrap back to the player's resources or similar
        stolenScrap = 0;  // Reset stolen scrap after returning
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
}
