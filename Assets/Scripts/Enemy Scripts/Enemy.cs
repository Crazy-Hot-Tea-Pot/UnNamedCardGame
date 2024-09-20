using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private string enemyName;

    public int maxHP;
    public int currentHP;

    public List<Drop> enemyDrops = new List<Drop>();

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

    // Start is called before the first frame update
    public virtual void Start()
    {
        currentHP = maxHP;
        gameObject.name = enemyName;

        Initialize();
    }

    public virtual void Initialize()
    {

    }
    public virtual void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log("Enemy " + name + " has taken " + damage + " damage and have " + currentHP + " remaining.");
        if(currentHP < 0)
        { 
            Die(); 
        }
    }
    public virtual void PerformAction(string actionName)
    {
        
    }

    public virtual void Die()
    {
        Debug.Log($"{enemyName} has been defeated!");
    }

    /// <summary>
    /// 
    /// </summary>
    public virtual void DropItems()
    {
        
    }
}
