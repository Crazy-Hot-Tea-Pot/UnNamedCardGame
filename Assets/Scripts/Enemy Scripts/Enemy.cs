using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.VolumeComponent;

public class Enemy : MonoBehaviour
{
    private string enemyName;

    private Animator animator;

    private NavMeshAgent agent;

    [Header("Enemy stats")]
    /// <summary>
    /// Max Hp of Enemy
    /// </summary>
    public int maxHP;
    [SerializeField]
    private int currentHp;   
    [SerializeField]
    private int shield;

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

    /// <summary>
    /// Reference to combat controller.
    /// </summary>
    public CombatController CombatController;

    /// <summary>
    /// Placeholder for testing.
    /// </summary>
    public GameObject dropPrefab;

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
            if (currentHp <= 0)
            {
                Die();
            }
        }
    }

    /// <summary>
    /// Drops for that enemy.
    /// </summary>
    //public List<Drop> enemyDrops = new List<Drop>();

    /// <summary>
    /// AttackRange is 3.0f by default;
    /// </summary>
    //public float attackRange = 3.0f;
   
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
            animator.SetBool("inCombat", value);
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

    public int DrainStacks { 
        get => drainStacks;
        protected set
        {
            drainStacks = value;
            if(drainStacks <= 0)
                IsDrained = false;
            else
                IsDrained = true;
        }
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    public virtual void Start()
    {        
        Initialize();
    }
    public virtual void Update()
    {
        if (InCombat)
        {
            if(CombatController.CanIMakeAction(this.gameObject))
            {
                PerformIntent();
            }
        }
    }

    public virtual void Initialize()
    {
        CurrentHP = maxHP;
        gameObject.name = EnemyName;

        CombatController = GameObject.FindGameObjectWithTag("CombatController").
            GetComponent<CombatController>();
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
    public virtual void PerformIntent()
    {
        CombatController.TurnUsed(this.gameObject);
    }
    /// <summary>
    /// Call when enemy die.
    /// </summary>
    public virtual void Die()
    {
        Debug.Log($"{enemyName} has been defeated!");
        DropItems();
        CombatController.RemoveCombadant(this.gameObject);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// Is called during enemy death.
    /// TODO drop items that enemy has.
    /// </summary>
    public virtual void DropItems()
    {
        //foreach (Drop drop in enemyDrops)
        //{
        //    GameObject instantiatedDrop = Instantiate(dropPrefab, Vector3.zero, Quaternion.identity);
        //}
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
}
