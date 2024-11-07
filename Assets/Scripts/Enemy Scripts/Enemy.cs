using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;

public class Enemy : MonoBehaviour
{
    public string enemyName;

    public GameObject enemyTarget;

    public float AttackRange;
    
    private float distanceToPlayer;

    public float DistanceToPlayer
    {
        get
        {
            distanceToPlayer= Vector3.Distance(transform.position, EnemyTarget.transform.position); ;
            return distanceToPlayer;
        }               
    }

    [Header("Enemy Components")]
    public Animator animator;
    public NavMeshAgent agent;    
    /// <summary>
    /// reference to player camera.
    /// </summary>
    public Camera playerCamera;
    /// <summary>
    /// reference to enemy canvas.
    /// </summary>
    public Canvas enemyCanvas;
    /// <summary>
    /// Name of Enemy Goes here.
    /// </summary>
    public TextMeshProUGUI EnemyNameBox;
    /// <summary>
    /// Enemy Health Bar.
    /// </summary>
    public Image healthBar;
    /// <summary>
    /// reference to effects Panel.
    /// </summary>
    public GameObject EffectsPanel;
    /// <summary>
    /// Prefabs of Effects enemy will use."Case sensitive"
    /// </summary>
    public List<GameObject> effectPrefabs;
    /// <summary>
    /// list of active effects.
    /// </summary>
    public List<GameObject> activeEffects;

    [Header("Enemy stats")]
    /// <summary>
    /// Max Hp of Enemy
    /// </summary>
    public int maxHP;
    private int currentHp;
    private int shield;
    private bool isTargeted;

    [Header("Status Effects")]
    [SerializeField]
    private bool inCombat;
    [SerializeField]
    private int galvanizedStacks;
    [SerializeField]
    private bool isGalvanized;
    [SerializeField]
    private int powerStacks;
    [SerializeField]
    private bool isPowered;
    [SerializeField]
    private int drainStacks;
    [SerializeField]
    private bool isDrained;

    [Header("Needed Assets")]
    public Shader outlineShader;
    private Shader defaultShader;
    private Renderer enemyRenderer;

    /// <summary>
    /// Reference to combat controller.
    /// </summary>
    public CombatController CombatController;

    /// <summary>
    /// This holds the cards we want to have dropped on death
    /// </summary>
    public List<NewChip> dropedCards;

    public GameObject EnemyTarget
    {
        get { return enemyTarget; }
        protected set
        {
            enemyTarget = value;
        }
    }

    /// <summary>
    /// Returns name of enemy
    /// </summary>
    public virtual string EnemyName
    {
        get
        {
            if (enemyName == null||enemyName=="")
            {
                EnemyName = this.GetComponent<Chip>().name;
                return enemyName;
            }
            else
                return enemyName;
        }
        protected set
        {
            enemyName = value;
            EnemyNameBox.SetText(enemyName);
        }
    }    

    /// <summary>
    /// Enemy Current Hp
    /// </summary>
    public int CurrentHP
    {
        get
        {
            return currentHp;
        }
        protected set
        {
            currentHp = value;

            //Update UI for enemy health
            UpdateEnemyHealthBar();

            if (currentHp <= 0)
            {
                currentHp = 0;
                Die();
            }
        }
    }
   
    /// <summary>
    /// Is the Enemy in Combat.
    /// </summary>
    public bool InCombat
    {
        get
        {
            return inCombat;
        }
        set
        {
            inCombat = value;
            //animator.SetBool("inCombat", value);
        }
    }
    /// <summary>
    /// Enemy Shield Amount.
    /// </summary>
    public int Shield
    {
        get
        {
            return shield;
        }
        protected set
        {
            shield = value;
            if (shield <= 0)
            {
                shield = 0;
            }
        }
    }
    /// <summary>
    /// Is enemy being targeted by player
    /// </summary>
    public bool IsTargeted
    {
        get
        {
            return isTargeted;
        }
        set
        {
            isTargeted = value;
            if (value)
            {
                enemyRenderer.material.shader = outlineShader;
                enemyRenderer.material.SetColor("_OutlineColor", Color.red);
                enemyRenderer.material.SetFloat("_OutlineWidth", 0.1f);
            }
            else
            {
                enemyRenderer.material.shader = defaultShader;
            }
        }
    }
    /// <summary>
    /// Is enemy GalvanizedStacks.
    /// Added for animation or effect later.
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

            // remove Galvanized from panel
            if (!isGalvanized)
                RemoveEffectIconFromPanel(Effects.Buff.Galvanize.ToString());
        }
    }
    /// <summary>
    /// Is enemy Drained.
    /// Added for animation or effect later.
    /// </summary>
    public bool IsDrained
    {
        get => isDrained;
        private set
        {
            isDrained = value;

            //Remove drained from panel
            if (!IsDrained)
                RemoveEffectIconFromPanel(Effects.Debuff.Drained.ToString());
        }
    }
    /// <summary>
    /// Amount of stacks of Drained the enemy have.
    /// </summary>
    public int DrainStacks
    {
        get
        {
            return drainStacks;
        }
        protected set
        {
            drainStacks = value;
            if (drainStacks <= 0)
                IsDrained = false;
            else
                IsDrained = true;
        }
    }
    /// <summary>
    /// Amount of stacks of Galvanize the enemy have.
    /// </summary>
    public int GalvanizedStacks
    {
        get => galvanizedStacks;
        protected set
        {
            galvanizedStacks = value;
            if (galvanizedStacks <= 0)
                IsGalvanized = false;
            else if(galvanizedStacks >= 1)
                IsGalvanized = true;
        }
    }
    /// <summary>
    /// Amount of stacks of Power the enemy have.
    /// </summary>
    public int PowerStacks { 
        get => powerStacks;
        protected set {  
            powerStacks = value;
            if (powerStacks <= 0)
                IsPowered = false;
            else
                IsPowered = true;
        } 
    }
    /// <summary>
    /// Is enemy powered.
    /// Added for animation or effect later.
    /// </summary>
    public bool IsPowered
    {
        get
        {
            return isPowered;
        }
        protected set
        {
            isPowered = value;

            if(!IsPowered)
                RemoveEffectIconFromPanel(Effects.Buff.Power.ToString());
        }
    }    

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        playerCamera = Camera.main;
        enemyRenderer = GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        Initialize();
    }

    public virtual void FixedUpdate()
    {
        
    }
    public virtual void Update()
    {

        if (InCombat)
        {
            if(CombatController.CanIMakeAction(this.gameObject))
            {
                //Look at player
                this.gameObject.transform.LookAt(EnemyTarget.transform);

                //Check if player is in range
                if (DistanceToPlayer <= AttackRange)
                {
                    agent.ResetPath();
                    PerformIntent();
                }
                else
                {                    
                    // move to player
                    agent.SetDestination(EnemyTarget.transform.position);
                }
            }
        }
    }

    public virtual void LateUpdate()
    {
        //if(playerCamera!= null)
        //    enemyCanvas.transform.LookAt(playerCamera.transform.position,Vector3.up);

        // trying this way to make it more smoother. only rotates on Y axis.
        if (playerCamera != null)
        {
            Vector3 direction = (playerCamera.transform.position - enemyCanvas.transform.position).normalized;
            direction.y = 0;  // Lock rotation to the Y-axis
            enemyCanvas.transform.rotation = Quaternion.LookRotation(-direction);
        }
    }

    public virtual void Initialize()
    {
        CurrentHP = maxHP;
        EnemyNameBox.SetText(EnemyName);
        gameObject.name = EnemyName;
        defaultShader=enemyRenderer.material.shader;

        CombatController = GameObject.FindGameObjectWithTag("CombatController").GetComponent<CombatController>();
        enemyTarget = GameObject.FindGameObjectWithTag("Player");
    }
   /// <summary>
   /// Is called when enemy is attacked by player.
   /// </summary>
   /// <param name="damage"></param>
    public virtual void TakeDamage(int damage)
    {
        // Plays sound of taking damage
        SoundManager.PlaySound(SoundFX.DamageTaken,this.gameObject.transform);

        // if has shield
        if (Shield > 0)
        {
            if (damage >= Shield)
            {
                damage -= Shield; 
                Shield = 0;
                Debug.Log("Enemy " + name + "Shield destroyed.");
            }
            else
            {
                // Reduce the shield by the damage amount
                Shield -= damage;
                // No remaining damage to apply to HP
                damage = 0;
            }
        }
        CurrentHP -= damage;
        Debug.Log("Enemy " + name + " has taken " + damage + " damage and have " + CurrentHP + " remaining.");
    }
    /// <summary>
    /// Base Perform Intent.
    /// Make sure the base is run after you perform an action.
    /// </summary>
    protected virtual void PerformIntent()
    {
        if(this.gameObject != null)
            CombatController.TurnUsed(this.gameObject);
    }
    /// <summary>
    /// Call when enemy die.
    /// </summary>
    public virtual void Die()
    {
        SoundManager.PlaySound(SoundFX.EnemyDefeated,this.gameObject.transform);

        Debug.Log($"{enemyName} has been defeated!");
        CombatController.RemoveCombadant(this.gameObject);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Called when round ends to apply buffs or debuffs.
    /// </summary>
    public virtual void RoundEnd()
    {
        if (GalvanizedStacks > 0)
        {
            Shield += GalvanizedStacks;
            IsGalvanized = false;
        }
    }
    
    /// <summary>
    /// Apply Debuffs to Enemy
    /// </summary>
    /// <param name="debuffToApply"></param>
    /// <param name="debuffStacks"></param>
    public virtual void ApplyDebuff(Effects.Debuff debuffToApply, int debuffStacks)
    {
        //Plays Debuff sound effect
        SoundManager.PlaySound(SoundFX.Debuff, this.gameObject.transform);

        switch (debuffToApply)
        {
            case Effects.Debuff.Drained:
                DrainStacks += debuffStacks;
                if (DrainStacks > 0)
                {
                    AddEffectIconToPanel(debuffToApply.ToString());
                }
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Apply Buff to Enemy
    /// </summary>
    /// <param name="buffToApply"></param>
    /// <param name="buffStacks"></param>
    public virtual void ApplyBuff(Effects.Buff buffToApply, int buffStacks)
    {        
        switch (buffToApply)
        {
            case Effects.Buff.Galvanize:
                GalvanizedStacks += buffStacks;
                if (GalvanizedStacks > 0)
                {
                    AddEffectIconToPanel(buffToApply.ToString());
                }
                break;
            case Effects.Buff.Power:
                PowerStacks+= buffStacks;
                if(PowerStacks>0)
                {
                    AddEffectIconToPanel(buffToApply.ToString());
                }
                break;
        }
    }

    /// <summary>
    /// This method allows us to change the UI in world enemy health bar based on the enemies health
    /// </summary>
    protected virtual void UpdateEnemyHealthBar()
    {
        //Rotate the ui to match camera angle
        //enemyCanvas.transform.rotation = new Quaternion(enemyCanvas.transform.position.x, playerCamera.transform.rotation.y, enemyCanvas.transform.position.z, 0);

        // Calculate health as a percentage
        float healthPercentage = (float)currentHp / maxHP;

        //Set the bars value
        healthBar.fillAmount = healthPercentage;
    }

    /// <summary>
    /// Check if the effect is already active.
    /// Method to instantiate and add effect to EffectsPanel.
    /// Instantiate effect icon and set its parent to EffectsPanel.
    /// Track active effects.
    /// </summary>
    /// <param name="effectPrefab"></param>
    protected void AddEffectIconToPanel(string effectName)
    {
        if (activeEffects.Exists(effect => effect.name == effectName))
            return;        

            GameObject effectPrefab = effectPrefabs.Find(prefab => prefab.name == effectName);

            try
            {
                GameObject effectInstance = Instantiate(effectPrefab, EffectsPanel.transform);
                effectInstance.name = effectName;
                activeEffects.Add(effectInstance);
            }
            catch
            {
                Debug.LogError("So Somebody fucked up.");
            }        
    }

    /// <summary>
    /// Method to remove an effect from EffectsPanel.
    /// Remove from tracking list.
    /// Destroy the effect GameObject.
    /// </summary>
    /// <param name="effect"></param>
    protected void RemoveEffectIconFromPanel(string effectName)
    {        
        GameObject effectToRemove = activeEffects.Find(effect => effect.name == effectName);
        try
        {
            if (effectToRemove != null)
            {
                activeEffects.Remove(effectToRemove);
                Destroy(effectToRemove);
            }
        }
        catch
        {
            Debug.LogError("So Somebody fucked up.");
        }
    }
    /// <summary>
    /// Give enemy shield.
    /// </summary>
    /// <param name="shieldAmount"></param>
    public virtual void ApplyShield(int shieldAmount)
    {
        //Restore Shield
        Shield += shieldAmount;

        Debug.Log("Shield Restored: " + shield);
    }
}