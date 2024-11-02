using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public string enemyName;

    public GameObject enemyTarget;

    public float AttackRange;
    
    [SerializeField]
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

    [Header("Enemy stats")]
    /// <summary>
    /// Starting Hp of Enemy
    /// </summary>
    public int StartingHP;
    [SerializeField]
    private int currentHp;
    [SerializeField]
    private int shield;
    [SerializeField]
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

    public GameObject EnemyTarget
    {
        get { return enemyTarget; }
        protected set
        {
            enemyTarget = value;
        }
    }


    /// <summary>
    /// UI Bar
    /// </summary>
    public Slider sliderBar;
    /// <summary>
    /// Camera Alignment
    /// </summary>
    public Camera cameraAlignment;
    /// <summary>
    /// Canvas for enemy health
    /// </summary>
    public Canvas enemyCanvas;

    /// <summary>
    /// Reference to combat controller.
    /// </summary>
    public CombatController CombatController;

    /// <summary>
    /// This holds the cards we want to have dropped on death
    /// </summary>
    public List<NewChip> dropedCards;

    /// <summary>
    /// Returns name of enemy
    /// </summary>
    public string EnemyName
    {
        get
        {
            return enemyName;
        }
        protected set
        {
            enemyName = value;
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
        set
        {
            currentHp = value;
            //Update UI for enemy health
            UIEnemyHealth();

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
        }
    }
    /// <summary>
    /// Amount of stacks of Drained the enemy have.
    /// </summary>
    public int DrainStacks
    {
        get => drainStacks;
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
        }
    }    

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        cameraAlignment = Camera.main;
        enemyRenderer = GetComponent<Renderer>();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        Initialize();
        //Sets the hp maximum for the slider bar
        UIEnemyMaxHealthStart();
    }

    public virtual void FixedUpdate()
    {
        //Update UI for enemy health
        //UIEnemyHealth();
        // Moved it to current health property so its not called repeatedly.
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

    public virtual void Initialize()
    {
        CurrentHP = StartingHP;
        gameObject.name = EnemyName;
        defaultShader=enemyRenderer.material.shader;

        CombatController = GameObject.FindGameObjectWithTag("CombatController").
            GetComponent<CombatController>();
        enemyTarget = GameObject.FindGameObjectWithTag("Player");
    }
   /// <summary>
   /// Is called when enemy is attacked by player.
   /// </summary>
   /// <param name="damage"></param>
    public virtual void TakeDamage(int damage)
    {
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
        CombatController.TurnUsed(this.gameObject);
    }
    /// <summary>
    /// Call when enemy die.
    /// </summary>
    public virtual void Die()
    {
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
        switch (debuffToApply)
        {
            case Effects.Debuff.Drained:
                drainStacks += debuffStacks;
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
                GalvanizedStacks+= buffStacks;
                break;
            case Effects.Buff.Power:
                PowerStacks+= buffStacks;
                break;
        }
    }

    /// <summary>
    /// This method allows us to change the UI in world enemy health bar based on the enemies health
    /// </summary>
    public void UIEnemyHealth()
    {
        //Rotate the ui to match camera angle
        enemyCanvas.transform.rotation = new Quaternion(enemyCanvas.transform.position.x, cameraAlignment.transform.rotation.y, enemyCanvas.transform.position.z, 0);
        //Set the bars value
        sliderBar.value = CurrentHP;
    }

    //Sets the max health of the slider bar
    public void UIEnemyMaxHealthStart()
    {
        sliderBar.maxValue = StartingHP;
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