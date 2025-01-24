
using System.Collections.Generic;
using UnityEngine;

public class SecurityDrone : Enemy
{
    [Header("Custom for Enemy type")]

    [SerializeField]
    private int intentsPerformed;

    [SerializeField]
    private int numberOfAlertDrones;

    [SerializeField]
    private bool isAlertDrone;

    /// <summary>
    /// Amount of Intents done.
    /// </summary>
    public int IntentsPerformed
    {
        get
        {
            return intentsPerformed;
        }
        private set
        {
            intentsPerformed = value;
        }
    }

    public int NumberOfAlertDrones
    {
        get
        {
            return numberOfAlertDrones;
        }
        private set
        {
            numberOfAlertDrones = value;
        }
    }
    /// <summary>
    /// If this is a drone thats been summoned by Alert.
    /// </summary>
    public bool IsAlertDrone
    {
        get
        {
            return isAlertDrone;
        }
        private set
        {
            isAlertDrone = value;
        }
    }
    // Start is called before the first frame update
    public override void Start()
    {
        EnemyName = "Security Drone";

        DroppedChips.Clear();

        //Add Common Chips Todrop
        DroppedChips = ChipManager.Instance.GetChipsByRarity(NewChip.ChipRarity.Common);

        base.Start();
    }
    protected override void PerformIntent()
    {
        UpdateIntentUI();

        IntentsPerformed++;

       if(IntentsPerformed > 5 && NumberOfAlertDrones < 3)
        {
            Alert();
        }
        else
        {            
            if(Random.Range(1,11) <= 3)
                Neutralize();
            else
                Ram();
        }

       //THIS IS NEEDED DON'T REMOVE. don't want that again.
       base.PerformIntent();

    }
    protected override Intent GetNextIntent()
    {
        if (IntentsPerformed > 5 && NumberOfAlertDrones < 3)
        {
            return new Intent("Alert", Color.blue, 0, "Summons another Security Drone");
        }
        else if (Random.Range(1, 11) <= 3)
        {
            return new Intent("Neutralize", Color.red, 7, "Deals damage and applies Drained");
        }
        else
        {
            return new Intent("Ram", Color.red, 12, "Deals heavy damage");
        }
    }
    /// <summary>
    /// Deals 12 Damage.
    /// Has a 70% chance of being called.
    /// </summary>
    private void Ram()
    {
        EnemyTarget.GetComponent<PlayerController>().DamagePlayerBy(12);
    }
    /// <summary>
    /// Deals 7 Damage.
    /// Applys Drain.
    /// Has a 30% chance of being called.
    /// </summary>
    private void Neutralize()
    {
        // Play Sound
        SoundManager.PlayFXSound(SoundFX.NeutralizeSecurityDrone);

        EnemyTarget.GetComponent<PlayerController>().DamagePlayerBy(7);
        EnemyTarget.GetComponent<PlayerController>().AddEffect(Effects.Debuff.Drained, 1);
    }
    /// <summary>
    /// Calls another Security Drone to the Combat Zone.
    /// Try to spawn one near by safely.
    /// </summary>
    private void Alert()
    {
        
        if (CombatController != null)
        {
            // Loop to find a clear position for the new drone
            Vector3 spawnOffset;
            Vector3 spawnPosition;
            int maxAttempts = 10;
            int attempt = 0;
            bool foundValidPosition = false;

            do {
                // Calculate a position next to the current drone
                spawnOffset = new Vector3(Random.Range(-2f, 2f), 0, Random.Range(-2f, 2f));
                spawnPosition = this.transform.position + spawnOffset;

                // Check if there's anything at the spawn position
                if (Physics.OverlapSphere(spawnPosition, 1f).Length == 0)
                {
                    foundValidPosition = true;
                }

                attempt++;

            } while (!foundValidPosition && attempt < maxAttempts);

            if (foundValidPosition)
            {

                GameObject AdditionalSecurityDrone = Instantiate(this.gameObject, spawnPosition, Quaternion.identity);
                AdditionalSecurityDrone.GetComponent<SecurityDrone>().IAmAlertDrone();

                CombatController.AddEnemyToCombat(AdditionalSecurityDrone);

                //Play Sound
                SoundManager.PlayFXSound(SoundFX.AlertSecurityDrone);
            }
            else
            {
                Debug.LogWarning("Failed to find valid spawn position Jayce -_- fix ya code.\n This is complicated I know but thats what testing is for.");
            }
        }
    }
    /// <summary>
    /// Different stuff for Alerted Drone
    /// </summary>
    public void IAmAlertDrone()
    {
        maxHP = 60;
        EnemyName = "Alert Drone";
        IsAlertDrone = true;
        IntentsPerformed = 0;

        base.Initialize();
    }
}
