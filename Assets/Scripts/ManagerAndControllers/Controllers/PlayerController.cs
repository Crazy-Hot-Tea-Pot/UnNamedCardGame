using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

// Controller for player this class is not the input class that is generated.
public class PlayerController : MonoBehaviour
{    

    //Camera in the scene
    private Camera mainCamera;

    //A list of combatEnemies in range for abilites
    public List<GameObject> abilityRangedEnemies;

    // The Select Action from inputAction class.
    private InputAction select;

    private bool inCombat;

    [Header("Needed Stuff")]    
    public PlayerInputActions playerInputActions;
    public NavMeshAgent agent;
    public Animator animator;
    public GameObject RipplePrefab;
    public GameObject RippleRunPrefab;


    [Header("Player stats")]
    #region PlayerStats 
    private int health;
    /// <summary>
    /// Returns PLayer HealthBar
    /// </summary>
    public int Health
    {
        get { return health; }
        private set
        {
            health = value;

            UiManager.Instance.UpdateHealth(Health, MaxHealth);

            if (health > maxHealth)
                health = maxHealth;
            else if (health <= 0)
            {
                health = 0;
                PlayerDie();
            }
        }
    }
    private int maxHealth;
    /// <summary>
    /// Returns max HealthBar
    /// </summary>
    public int MaxHealth
    {
        get { return maxHealth; }
        private set
        {
            maxHealth = value;
        }
    }
    private int shield;
    /// <summary>
    /// Player ShieldBar amount
    /// </summary>
    public int Shield
    {
        get
        {
            return shield;
        }
        private set
        {
            shield = value;

            if (shield > maxShield)
                maxShield = value;

            if (shield <= 0)
            {
                shield = 0;
                maxShield = 100;
            }

            UiManager.Instance.UpdateShield(Shield, MaxShield);
        }
    }
    private int maxShield=100; 
    
    /// <summary>
    /// Max amount of ShieldBar currently.
    /// </summary>
    public int MaxShield
    {
        get
        {
            return maxShield;
        }
        set
        {
            maxShield = value;
        }
    }
    private int energy;
    /// <summary>
    /// Returns PlayerEnergy
    /// </summary>
    public int Energy
    {
        get { return energy; }
        private set
        {
            energy = value;

            if (energy > maxEnergy)
                energy = value;
            else if (energy <= 0)
                energy = 0;

            UiManager.Instance.UpdateEnergy(Energy, MaxEnergy);
        }
    }
    private readonly int maxEnergy=50;
    /// <summary>
    /// Returns max energy
    /// </summary>
    public int MaxEnergy
    {
        get { return maxEnergy; }
    }
    private int scrap;
    /// <summary>
    /// Player Scrap
    /// </summary>
    public int Scrap
    {
        get
        {
            return scrap;
        }
        private set
        {
            scrap = value;
            if (scrap < 0)
                scrap = 0;
        }
    }
    #endregion

    [Header("Player Speed")]
    public float walkSpeed = 3.5f;
    public float runSpeed = 7.0f;
    // Speed threshold to detect running
    public float runThreshold = 5.0f;
    // Max time interval for double-click detection
    public float doubleClickTime = 0.3f; 
    private float lastClickTime;


    [Header("Status Effects")]
    [Header("Buffs")]
    #region Buffs
    private bool isGalvanized;    
    private int galvanizedStack;
    /// <summary>
    /// Returns if player is Galvanized.
    /// </summary>
    public bool IsGalvanized
    {
        get
        {
            return isGalvanized;
        }
        private set
        {
            isGalvanized = value;
        }
    }
    /// <summary>
    /// Returns how many stacks of Galvanized the player has.
    /// </summary>
    public int GalvanizedStack
    {
        get => galvanizedStack;
        private set
        {
            galvanizedStack = value;
            if (galvanizedStack <= 0)
            {
                IsGalvanized = false;
                galvanizedStack = 0;
            }
            else
                IsGalvanized = true;
        }
    }
    private bool isPowered;    
    private int poweredStacks;
    /// <summary>
    /// Returns if player is Powered.
    /// </summary>
    public bool IsPowered
    {
        get => isPowered;
        private set => isPowered = value;
    }
    /// <summary>
    /// Returns how many stacks of power the player has.
    /// </summary>
    public int PoweredStacks
    {
        get => poweredStacks;
        private set
        {
            poweredStacks = value;
            if (poweredStacks <= 0)
            {
                IsPowered = false;
                poweredStacks = 0;
            }
            else
                IsPowered = true;
        }
    }
    #endregion

    [Header("DeBuffs")]
    #region Debuffs    
    private bool isDrained;
    /// <summary>
    /// Returns if the player is drained.
    /// </summary>
    public bool IsDrained
    {
        get
        {
            return isDrained;
        }
        private set
        {
            isDrained = value;
        }
    }
    private int drainedStacks;
    /// <summary>
    /// Returns how many stacks of drained the player has.
    /// </summary>
    public int DrainedStacks
    {
        get
        {
            return drainedStacks;
        }
        private set
        {
            drainedStacks = value;
            if (drainedStacks <= 0)
            {
                IsDrained = false;
                drainedStacks = 0;
            }
            else
                IsDrained = true;
        }
    }
    private bool isWornDown;
    /// <summary>
    /// Returns if the player is in worndown state.
    /// </summary>
    public bool IsWornDown
    {
        get => isWornDown;
        private set => isWornDown = value;
    }
    private int wornDownStacks;
    /// <summary>
    /// Returns how many stacks of worn down the player has.
    /// </summary>
    public int WornDownStacks
    {
        get
        {
            return wornDownStacks;
        }
        private set
        {
            wornDownStacks = value;
            if (wornDownStacks <= 0)
            {
                IsWornDown = false;
                wornDownStacks = 0;
            }
            else
                IsWornDown = true;
        }
    }
    private bool isJammed;
    /// <summary>
    /// Returns if player is Jammed.
    /// </summary>
    public bool IsJammed
    {
        get => isJammed;
        private set => isJammed = value;
    }

    private int jammedStacks;
    /// <summary>
    /// Returns how many stacks of jammed the palyer has.
    /// </summary>
    public int JammedStacks
    {
        get
        {
            return jammedStacks;
        }
        private set
        {
            jammedStacks = value;
            if (jammedStacks <= 0)
            {
                IsJammed = false;
                jammedStacks = 0;
            }
            else
                IsJammed = true;
        }
    }
    #endregion

    [Header("Effects")]
    #region Effects
    private bool nextChipActivatesTwice;
    /// <summary>
    /// Used to apply effect of activing a chip twice.
    /// </summary>
    public bool NextChipActivatesTwice
    {
        get
        {
            return nextChipActivatesTwice;
        }
        private set
        {
            nextChipActivatesTwice = value;
        }
    }
    private bool isImpervious;
    /// <summary>
    /// Used to apply effect to not take damage.
    /// </summary>
    public bool IsImpervious
    {
        get
        {
            return isImpervious;
        }
        private set
        {
            isImpervious = value;
        }
    }

    /// <summary>
    /// List of current Effects the player has active.
    /// </summary>
    public List<Effects.Effect> ListOfActiveEffects = new List<Effects.Effect>();
    #endregion                    

    // Awake is called when instance is being loaded
    void Awake()
    {
        // assign player Input class
        playerInputActions = new PlayerInputActions();

        // Automatically finds the camera
        mainCamera = Camera.main;

        agent = GetComponent<NavMeshAgent>();

    }

    void Start()
    {
       
        agent.updateRotation = false;
        agent.speed = walkSpeed;

        Initialize();
    }

    void Update()
    {
        TargetAbilityRangedVisual();
    }

    /// <summary>
    /// We will do animations in Late Update.
    /// </summary>
    void LateUpdate()
    {
        // Determine if player is running or walking based on the agent's current speed
        bool isRunning = agent.velocity.magnitude > runThreshold;
        bool isWalking = agent.velocity.magnitude > 0.1f && !isRunning;

        // Set animator bools based on detected movement state
        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("IsRunning", isRunning);
    }

    /// <summary>
    /// Initialize Player
    /// </summary>
    void Initialize()
    {
        health = DataManager.Instance.CurrentGameData.Health;
        MaxHealth = DataManager.Instance.CurrentGameData.MaxHealth;
        Scrap = DataManager.Instance.CurrentGameData.Scraps;
        Energy = MaxEnergy;
    }

    /// <summary>
    /// Enables
    /// </summary>
    private void OnEnable()
    {
        select = playerInputActions.Player.Select;
        select.Enable();
        select.performed += OnClick;
    }

    /// <summary>
    /// Disables
    /// </summary>
    private void OnDisable()
    {
        select.Disable();
        select.performed -= OnClick;
    }

    /// <summary>
    /// On click is run everytime the user clicks into the scene.
    /// Using Physics raycast.
    /// Depends on the result it will either:
    /// Assign the selectedObject .
    /// Move the selectedObejct to the position on Ground.
    /// </summary>
    private void OnClick(InputAction.CallbackContext context)
    {
        //Put in Coroutine to cancel out errors
        StartCoroutine(HandleClick());
    }
    private IEnumerator HandleClick()
    {
        // Wait until the end of the frame to ensure UI state is updated
        yield return null;

        // Check if the pointer is over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("[PlayerController] Click detected on UI. Ignoring.");
            yield break; // Exit if the click is on the UI
        }

        // Create a Ray from the current mouse position
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        // Define LayerMask to exclude the "Player" layer
        int layerMask = LayerMask.GetMask("Ground"); // Add more layers if needed

        // Perform the raycast
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            // Ignore clicks on objects other than the ground
            if (hit.collider.CompareTag("Ground"))
            {
                HandleGroundClick(hit.point);
            }
            else
            {
                Debug.Log("[PlayerController] Click detected on a non-ground object.");
            }
        }
        else
        {
            Debug.Log("[PlayerController] No valid object detected.");
        }
    }
    private void HandleGroundClick(Vector3 clickPoint)
    {
        GameObject tempIndicator;
        float currentTime = Time.time;

        // Double-click detection
        if ((currentTime - lastClickTime) <= doubleClickTime)
        {
            agent.speed = runSpeed;
            tempIndicator = RippleRunPrefab;
        }
        else
        {
            agent.speed = walkSpeed;
            tempIndicator = RipplePrefab;
        }

        // Create ripple effect at the click point
        GameObject ClickIndicator = Instantiate(tempIndicator, clickPoint, Quaternion.identity);

        // Make the ripple effect face the player
        ClickIndicator.transform.LookAt(this.transform.position);

        // Destroy the ripple effect after 2 seconds
        Destroy(ClickIndicator, 2f);

        // Move the player to the clicked position
        agent.SetDestination(clickPoint);
        transform.LookAt(clickPoint);

        // Update the last click time
        lastClickTime = currentTime;
    }

    /// <summary>
    /// heal player by amount
    /// </summary>
    /// <param name="amountHeal"></param>
    public void Heal(int amountHeal)
    {
            //Heal the player using the getter and setter value
            //This will also check for over healing and correct
            Health += amountHeal;

        //Debug log healing
        Debug.Log("Health Restored: " + health);
        
    }

    /// <summary>
    /// Heal to max hp.
    /// </summary>
    public void FullHeal()
    {
        Health = maxHealth;
    }
    /// <summary>
    /// Changes the max HealthBar value
    /// </summary>
    /// <param name="amount"></param>
    public void UpgradeMaxHealth(int amount)
    {
        MaxHealth += amount;
    }

    /// <summary>
    /// Give player shield.
    /// </summary>
    /// <param name="shieldAmount"></param>
    public void ApplyShield(int shieldAmount)
    {
        //Restore ShieldBar
        Shield += shieldAmount;

    }
    [ContextMenu("Shield Text")]
    public void TestShield()
    {
        ApplyShield(10);
    }
    [ContextMenu("Damage Test")]
    public void TestDamage()
    {
        DamagePlayerBy(5);
    }

    /// <summary>
    /// Give player energy.
    /// </summary>
    /// <param name="energyAmount"></param>
    public void RecoverEnergy(int energyAmount)
    {
            //Restore energy this will inside of the getter and setter variable check if we are over energizing and correct
            Energy += energyAmount;

        Debug.Log("Energy Restored: " + energy);
    }

    /// <summary>
    /// Recover energy to max.
    /// </summary>
    public void RecoverFullEnergy()
    {
        Energy = maxEnergy;
    }

    /// <summary>
    /// Deal Damage to player.
    /// </summary>
    /// <param name="damage">Amount of Damage as Int.</param>
    public void DamagePlayerBy(int damage)
    {
        //if Impervious
        if (IsImpervious)
        {
            return;
        }
        else
        {
            int modifiedDamage = damage;

            if (IsWornDown)
            {
                modifiedDamage = Mathf.CeilToInt(damage * 1.3f);
            }
            // if has shield
            if (Shield > 0)
            {                
                if (modifiedDamage >= Shield)
                {
                    modifiedDamage -= Shield;
                    Shield = 0;
                    Debug.Log(name + "Shield destroyed.");
                }
                else
                {
                    // Reduce the shield by the damage amount
                    Shield -= modifiedDamage;
                    Debug.Log("Shield "+Shield+ " Took Damage"+ modifiedDamage);
                    // No remaining damage to apply to HP
                    modifiedDamage = 0;
                }
            }
            Health = Health - modifiedDamage;
            

            //Play Sound
            SoundManager.PlayFXSound(SoundFX.DamageTaken, this.transform);
        }
    }

    /// <summary>
    /// Apply Skill Effect
    /// </summary>
    /// <param name="effect"></param>
    public void ApplyEffect(Effects.Effect effect)
    {
        switch (effect)
        {
            case Effects.Effect.Motivation:
                NextChipActivatesTwice = true;
                break;
            case Effects.Effect.Impervious:
                IsImpervious = true;
                break;
            default:
                break;
        }

        ListOfActiveEffects.Add(effect);

    }

    /// <summary>
    /// Apply Buff to Player
    /// </summary>
    /// <param name="buffToApply"></param>
    /// <param name="buffStacks"></param>
    public void ApplyEffect(Effects.Buff buffToApply, int buffStacks)
    {
        //Play buff sound
        SoundManager.PlayFXSound(SoundFX.Buff);

        switch (buffToApply)
        {
            case Effects.Buff.Galvanize:
                GalvanizedStack += buffStacks;
                break;
            case Effects.Buff.Power:
                PoweredStacks += buffStacks;
                break;
        }
    }

    /// <summary>
    /// Apply Debuff to player.
    /// </summary>
    /// <param name="deBuffToApply"></param>
    /// <param name="deBuffStacks"></param>
    public void ApplyEffect(Effects.Debuff deBuffToApply, int deBuffStacks)
    {
        //Play sound effect
        SoundManager.PlayFXSound(SoundFX.Debuff);

        switch (deBuffToApply)
        {           
            case Effects.Debuff.Drained:
                DrainedStacks += deBuffStacks;
                break;
            case Effects.Debuff.WornDown:
                WornDownStacks += deBuffStacks;
                break;
            case Effects.Debuff.Jam:
                JammedStacks += deBuffStacks;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Remove Debuffs on player.
    /// </summary>
    /// <param name="deBuffToRemove"></param>
    /// <param name="amount"></param>
    /// <param name="removeAll"></param>
    public void RemoveEffect(Effects.Debuff deBuffToRemove, int amount,bool removeAll)
    {
        switch (deBuffToRemove)
        {
            case Effects.Debuff.Drained:
                if (removeAll)
                    DrainedStacks = 0;
                else
                    DrainedStacks-= amount;
                break;            
            case Effects.Debuff.Jam:
                if (removeAll)
                    JammedStacks = 0; 
                else
                    JammedStacks-= amount;
                break;
            case Effects.Debuff.WornDown:
                if (removeAll)
                    WornDownStacks = 0;
                else
                    WornDownStacks-= amount;
                break;                
            default:
                Debug.LogWarning("Debuff not found.");
                break;
        }        
    }

    /// <summary>
    /// Remove an special affect from being isActive.
    /// </summary>
    /// <param name="effect"></param>
    public void RemoveEffect(Effects.Effect effect)
    {
        switch (effect)
        {
            case Effects.Effect.Motivation:
                NextChipActivatesTwice = false;
                break;
            default:
                Debug.LogWarning("Effect hasn't been programmed.");
                break;
        }

        ListOfActiveEffects.Remove(effect);
    }

    /// <summary>
    /// Adds Scraps
    /// </summary>
    /// <param name="amount"></param>
    public void GainScrap(int amount)
    {
        Scrap+=amount;
    }

    /// <summary>
    /// Returns the Scraps stolen or whats left.
    /// </summary>
    /// <param name="amount">the amount of Scraps want to steal</param>
    /// <returns></returns>
    public int TakeScrap(int amount)
    {
        if (Scrap < amount)
        {
            int scrapleft = Scrap;
            Scrap -= amount;
            return scrapleft;
        }
        else
        {
            Scrap = Scrap - amount;
            return amount;
        }
    }

    /// <summary>
    /// Stuff to do at start of players turn.
    /// </summary>
    public void StartTurn()
    {
        //Remove shield
        if (Shield > 0)        
            Shield = 0;

        //Remove buffs by 1
        GalvanizedStack--;        
    }

    /// <summary>
    /// Called player ends their turn
    /// </summary>
    public void EndTurn()
    {
        //Restore energy to full
        RecoverFullEnergy();

        //Remove debuffs by 1
        DrainedStacks--;
        WornDownStacks--;
        JammedStacks--;
    }

    /// <summary>
    /// Called when round ends to apply buffs or debuffs.
    /// Buffs Stack don't go away.
    /// Debuffs Stack go away every round.
    /// </summary>
    public void RoundEnd()
    {       
        if (galvanizedStack > 0)
        {
            ApplyShield(galvanizedStack);            
        }               
    }

    /// <summary>
    /// Spend energy for ability
    /// </summary>
    /// <param name="loss"></param>
    public void PlayedAbility(int loss)
    {
        SoundManager.PlayFXSound(SoundFX.Charging_Up, this.transform);

        //If energy - loss is greater then 0 or equal to 0 then continue
        if(Energy - loss >= 0)
        {
            Energy -= loss;
        }
        //If not then don't be negative
        else
        {
            Energy = 0;
        }
    }

    /// <summary>
    /// Call when player is dead.
    /// Does stuff for game over.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void PlayerDie()
    {
        Debug.Log("Player died, game over.");

        //Reset HealthBar, energy and other stuff for now.
        Health = maxHealth;
        Energy = maxEnergy;
        GainScrap(200);

        GameManager.Instance.EndCombat();

        // for now just restart the scene.
        GameManager.Instance.RequestScene(Levels.Title);
    }    

    #region rangesForAbilites
    private void OnTriggerEnter(Collider other)
    {
        //If the hit is an enemy
        if(other.tag == "Enemy")
        {
            //Add to the list
            abilityRangedEnemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //If an enemy leaves the target range
        if(other.tag == "Enemy")
        {
            //remove from the list
            abilityRangedEnemies.Remove(other.gameObject);
            //Untarget it
            try
            {
                //Enemy is set as untargeted
                other.GetComponent<Enemy>().SetTarget(TargetingType.Ability, false);
            }
            catch
            {
                //Assume we couldn't find the enemy
                Debug.LogWarning("We couldn't find the right enemy object it had no enemy script attached in player controller when leaving range");
            }
        }
    }

    public void TargetAbilityRangedVisual()
    {
        foreach (GameObject enemy in abilityRangedEnemies)
        {
            //Try to deal damage one by one to each enemy
            try
            {
                //Enemy is set as targed
                enemy.GetComponent<Enemy>().SetTarget(TargetingType.Ability, true);
            }
            catch
            {
                //Assume we couldn't find the enemy
                Debug.LogWarning("We couldn't find the right enemy object it had no enemy script attached in player controller");
            }
        }
    }
    #endregion    
}
