using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.Rendering.VolumeComponent;

public class Enemy : MonoBehaviour
{
    private string enemyName;

    [SerializeField]
    private int currentHp;

    private Animator animator;

    private NavMeshAgent agent;

    [SerializeField]
    private bool inCombat;

    [SerializeField]
    private int shield;

    /// <summary>
    /// Reference to combat controller.
    /// </summary>
    public CombatController CombatController;

    /// <summary>
    /// Placeholder for testing.
    /// </summary>
    public GameObject dropPrefab;

    /// <summary>
    /// Max Hp of Enemy
    /// </summary>
    public int maxHP;

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
    public List<Drop> enemyDrops = new List<Drop>();

    /// <summary>
    /// AttackRange is 3.0f by default;
    /// </summary>
    public float attackRange = 3.0f;

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
        foreach(Drop drop in enemyDrops)
        {
            GameObject instantiatedDrop = Instantiate(dropPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
