/// <summary>
/// Made this incase we do chasing later
/// </summary>
public class ChaseState : EnemyStateMachine
{
    public ChaseState(Enemy enemy) : base(enemy) { }

    public override void Enter()
    {
        // Chase logic
        //enemy.StartChasingPlayer();
    }

    public override void Update()
    {
        //if (enemy.IsInRangeToAttack())
        //{
        //    enemy.TransitionToState(new AttackState(enemy));
        //}
    }

    public override void Exit()
    {
        // Cleanup or reset logic
    }
}
