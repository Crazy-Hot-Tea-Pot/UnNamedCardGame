using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public Enemy enemy;

    public EnemyStateMachine(Enemy enemy)
    {
        this.enemy = enemy;
    }

    /// <summary>
    /// Idle
    /// </summary>
    public virtual void Enter()
    {

    }
    /// <summary>
    /// Can do player detection here
    /// </summary>
    public virtual void Update()
    {

    }
    /// <summary>
    /// Clean up etc
    /// </summary>
    public virtual void Exit()
    {

    }

}
