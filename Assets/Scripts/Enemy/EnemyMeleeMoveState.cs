using UnityEngine;

public class EnemyMeleeMoveState : EnemyState
{
    private EnemyMelee enemy;

    private Vector3 destination;

    public EnemyMeleeMoveState(
        Enemy enemyBase,
        EnemyStateMachine stateMachine,
        string animatorBoolName
    )
        : base(enemyBase, stateMachine, animatorBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();

        destination = enemy.GetPatrolDestination();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.agent.SetDestination(destination);

        if (enemy.agent.remainingDistance <= 1)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
