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
    /// Enemy Name
    /// </summary>
    public string EnemyName
    {
        get
        {
            return enemyName;
        }
        set
        {
            enemyName = value;
        }
    }

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


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        CurrentHP = maxHP;
        gameObject.name = enemyName;

        Initialize();
    }

    public virtual void Initialize()
    {

    }
   
    public virtual void TakeDamage(int damage)
    {
        CurrentHP -= damage;
        Debug.Log("Enemy " + name + " has taken " + damage + " damage and have " + CurrentHP + " remaining.");
    }
    /// <summary>
    /// Base Perform Intent.
    /// when overriding make sure to tell combatcontroller you made a turn.
    /// </summary>
    public virtual void PerformNextIntent()
    {
        if(GameObject.FindGameObjectWithTag("CombatController").
            GetComponent<CombatController>().CanIMakeAction(this.gameObject))
        {
            // run override code
        }
        else
        {
            Debug.Log("Can't make action.");
            return;
        }
    }
    /// <summary>
    /// Call when enemy die.
    /// </summary>
    public virtual void Die()
    {
        Debug.Log($"{enemyName} has been defeated!");
        Destroy(this.gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void DropItems()
    {
        
    }
}
