using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

// Controller for player this class is not the input class that is generated.
public class PlayerController : MonoBehaviour
{    

    //Camera in the scene
    private Camera mainCamera;

    //A list of enemies in range for abilites
    public List<GameObject> abilityRangedEnemies;

    // The Select Action from inputAction class.
    private InputAction select;

    private bool inCombat;

    private bool isInteracting;

    [Header("Needed Stuff")]    
    public PlayerInputActions playerInputActions;
    public NavMeshAgent agent;
    public Animator animator;
    public GameObject RipplePrefab;
    public GameObject RippleRunPrefab;


    [Header("Player stats")] 
    
    private int health;
    private int maxHealth;    
    private int shield;
    private int maxShield=0;
    private int energy;
    //Just hard coded this since it never changes.
    private int maxEnergy=50;    
    private int scrap;

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
    private bool isGalvanized;    
    private int galvanizedStack;    
    private bool isPowered;    
    private int poweredStacks;

    [Header("DeBuffs")]    
    private bool isGunked;    
    private int gunkStacks;    
    private int amountOfTurnsGunkedLeft;    
    private bool isDrained;    
    private int drainedStacks;        
    private bool isWornDown;    
    private int wornDownStacks;    
    private bool isJammed;    
    private int jammedStacks;

    [Header("Effects")]    
    private bool nextChipActivatesTwice;    
    private bool isImpervious;

    /// <summary>
    /// Returns if player is in combat.
    /// </summary>
    public bool InCombat
    {
        get { return inCombat; }
        set
        {
            inCombat = value;
        }
    }

    /// <summary>
    /// Is player is interacting with object.
    /// Stops playing from being to move.
    /// </summary>
    public bool IsInteracting
    {
        get
        {
            return isInteracting;
        }
        set
        {
            isInteracting = value;
        }
    }

    /// <summary>
    /// Returns PLayer Health
    /// </summary>
    public int Health
    {
        get { return health; }
        private set { 
            health = value;

            GameObject.FindGameObjectWithTag("PlayerCanvas").
                GetComponent<PlayerUIManager>().UpdateHealth();

            if (health > maxHealth)
                health = maxHealth;
            else if (health <= 0)
            {
                health = 0;
                PlayerDie();
            }            
        }
    }

    /// <summary>
    /// Player Shield amount
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
                maxShield = 0;
            }

            GameObject.FindGameObjectWithTag("PlayerCanvas").
               GetComponent<PlayerUIManager>().UpdateShield();
        }
    }
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

    /// <summary>
    /// Returns PlayerEnergy
    /// </summary>
    public int Energy
    {
        get { return energy; }
        private set
        {
            energy = value;

            if(energy > maxEnergy)
                energy = value;
            else if(energy <= 0)
                energy = 0;

            GameObject.FindGameObjectWithTag("PlayerCanvas").GetComponent<PlayerUIManager>().UpdateEnergy(Energy,MaxEnergy);
        }
    }

    /// <summary>
    /// Returns max energy
    /// </summary>
    public int MaxEnergy
    {
        get { return maxEnergy; }
        private set
        {
            maxEnergy = value;
        }
    }

    /// <summary>
    /// Returns max Health
    /// </summary>
    public int MaxHealth
    {
        get { return maxHealth; }
        private set
        {
            maxHealth = value;
        }
    }
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

    /// <summary>
    /// Returns if player is Powered.
    /// </summary>
    public bool IsPowered { 
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

    /// <summary>
    /// Returns if player is gunked.
    /// </summary>
    public bool IsGunked {
        get => isGunked;
        private set {
         isGunked = value;
        }
    }

    /// <summary>
    /// Returns how many stacks of gunk the player has.
    /// </summary>
    public int GunkStacks
    {
        get => gunkStacks;
        set
        {
            gunkStacks = value;

            if (gunkStacks >= 3)
            {
                IsGunked = true;
                gunkStacks = 0;
                AmountOfTurnsGunkedLeft = 1;
            }
            else if (gunkStacks <= 0)
                gunkStacks = 0;
        }
    }
    /// <summary>
    /// Use this to prevent player from attacking when gunked.
    /// </summary>
    public int AmountOfTurnsGunkedLeft
    {
        get => amountOfTurnsGunkedLeft;
        private set
        {
            amountOfTurnsGunkedLeft = value;
            if (amountOfTurnsGunkedLeft <= 0)
                IsGunked = false;
        }
    }

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

    /// <summary>
    /// Returns if the player is drained.
    /// </summary>
    public bool IsDrained { 
        get
        {
            return isDrained;
        }
        private set
        {
            isDrained = value;
        }
    }

    /// <summary>
    /// Returns if the player is in worndown state.
    /// </summary>
    public bool IsWornDown { 
        get => isWornDown;
        private set => isWornDown = value; 
    }

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

    /// <summary>
    /// Returns if player is Jammed.
    /// </summary>
    public bool IsJammed { 
        get => isJammed;
        private set => isJammed = value; 
    }           

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
        if (!InCombat && !IsInteracting)
        {
            //temp to hold which gameobject ripple effect to spawn
            GameObject tempIndicator;

            // Double-click detection: Check if the same point was clicked within the doubleClickTime
            float currentTime = Time.time;

            if ((currentTime - lastClickTime) <= doubleClickTime)
            {
                // Double-click detected: Set speed to run
                agent.speed = runSpeed;
                tempIndicator = RippleRunPrefab;
            }
            else
            {
                agent.speed = walkSpeed;
                tempIndicator = RipplePrefab;
            }


            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            // Create a LayerMask that excludes the "Player" layer
            int layerMask = ~LayerMask.GetMask("Player");
            int layerMask2 = ~LayerMask.GetMask("Ignore Raycast");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask2))
                {

                    if (hit.collider.CompareTag("Ground"))
                    {

                        if (agent != null)
                        {
                            // UNCOMMENT BELOW IS STILL MOONWALKING
                            //// Clears the current path, stops the agent
                            //agent.ResetPath();
                            //// Resets any existing velocity
                            //agent.velocity = Vector3.zero;

                            // Set destination and make player face the destination

                            //Spawn Ripple effect at location.
                            GameObject ClickIndicator = Instantiate(tempIndicator,hit.point, Quaternion.identity);

                            //make the ! be at player
                            ClickIndicator.transform.LookAt(this.transform.position);

                            //Destroy it after 2 seconds
                            Destroy(ClickIndicator, 2f);


                            agent.SetDestination(hit.point);
                            this.gameObject.transform.LookAt(hit.point);
                        }
                    }
                }
            }

            // Update the time of the last click
            lastClickTime = currentTime;
        }
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
    /// Changes the max Health value
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
        //Restore Shield
        Shield += shieldAmount;

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
    public void TakeDamage(int damage)
    {
        //if Impervious
        if (IsImpervious)
        {            
        }
        else
        {
            // if has shield
            if (Shield > 0)
            {
                if (damage >= Shield)
                {                    
                    damage -= Shield;
                    Shield = 0;
                    Debug.Log(name + "Shield destroyed.");
                }
                else
                {
                    // Reduce the shield by the damage amount
                    Shield -= damage;
                    Debug.Log("Shield "+Shield+ " Took Damage"+damage);
                    // No remaining damage to apply to HP
                    damage = 0;
                }
            }
            Health = Health - damage;
            

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
            case Effects.Debuff.Gunked:
                GunkStacks += deBuffStacks;
                break;
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
            case Effects.Debuff.Gunked:
                if (removeAll)
                    GunkStacks = 0;
                else
                    GunkStacks-= amount;
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
        PoweredStacks--;
    }

    /// <summary>
    /// Called player ends their turn
    /// </summary>
    public void EndTurn()
    {
        //Restore energy to full
        RecoverFullEnergy();

        //Remove debuffs by 1
        GunkStacks--;
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
        if (isGunked)
        {
            AmountOfTurnsGunkedLeft--;
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

        //Reset Health, energy and other stuff for now.
        Health = maxHealth;
        Energy = maxEnergy;
        GainScrap(200);

        // for now just restart the scene.
        GameManager.Instance.RequestScene(Levels.Level1);
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
