using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Looter : Enemy
{
    [Header("Custom for Enemy type")]
    private int stolenScrap = 0;
    /// <summary>
    /// To keep track of stolen Scraps
    /// </summary>
    public int StolenScrap
    {
        get
        {
            return stolenScrap;
        }
        private set
        {
            stolenScrap = value;
        }
    }

    // To track the number of Swipes performed
    private int swipeCount = 0;

    private bool isShrouded;

    public bool IsShrouded
    {
        get { return isShrouded; }
        private set { isShrouded = value; }
    }

    // Start is called before the first frame update
    public override void Start()
    {
        swipeCount = 0;
        StolenScrap = 0;

        base.Start();
    }

    protected override void PerformIntent()
    {       
        // Since 100% on first chance just made it this way.

        if (swipeCount < 3) // First three turns are Swipe
        {
            Swipe();

            if (swipeCount < 3)
                UpdateIntentUI("Swipe", Color.red);
            else
                UpdateIntentUI("Shroud", Color.blue);
        }
        else if (swipeCount == 3 && !IsShrouded) // After three Swipes, do Shroud
        {
            Shroud();
            UpdateIntentUI("Escape", Color.red);
        }
        else if (IsShrouded) // After Shroud, perform Escape
        {
            Escape();
        }

        base.PerformIntent();
    }
    /// <summary>
    /// Return all stolen Scraps upon killing
    /// </summary>
    public override void Die()
    {        
        ReturnStolenScrap();
        base.Die();
    }    

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }
    /// <summary>
    ///  After the 3rd Swipe, perform Shroud
    /// </summary>
    private void Swipe()
    {
        Debug.Log($"{EnemyName} performs Swipe, dealing 4 damage and stealing 5 Scrap.");
        swipeCount++;

        StolenScrap += EnemyTarget.GetComponent<PlayerController>().TakeScrap(5);

        // Empower Swipe
        if (PowerStacks > 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().TakeDamage(6 + PowerStacks);
            PowerStacks = 0;
        }
        // Drained Swipe
        else if (DrainedStacks > 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().TakeDamage(Mathf.FloorToInt(6 - 0.8f));
            DrainedStacks--;
        }
        // Default Swipe
        else
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().TakeDamage(6);
        }

    }

    private void Shroud()
    {
        Debug.Log($"{EnemyName} performs Shroud, gaining 10 Shield.");
        Shield += 10;

        isShrouded = true;
    }

    private void Escape()
    {
        Debug.Log($"{EnemyName} performs Escape, exiting the fight with {StolenScrap} Scrap.");
        CombatController.RemoveCombadant(this.gameObject);
        Destroy(this.gameObject);
    }
    private void ReturnStolenScrap()
    {
        Debug.Log($"{EnemyName} returns {StolenScrap} Scrap upon defeat.");

        EnemyTarget.GetComponent<PlayerController>().GainScrap(StolenScrap);

        // Reset stolen Scraps after returning
        StolenScrap = 0;
    }
    protected override void CombatStart()
    {
        base.CombatStart();

        UpdateIntentUI("Swipe", Color.red);
    }
}