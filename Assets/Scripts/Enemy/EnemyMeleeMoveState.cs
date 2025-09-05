using UnityEngine;
using UnityEngine.AI;

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

        enemy.agent.speed = enemy.moveSpeed;

        destination = enemy.GetPatrolDestination();
        enemy.agent.SetDestination(destination);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerInAggressionRange())
        {
            stateMachine.ChangeState(enemy.recoveryState);
            return;
        }

        enemy.transform.rotation = enemy.FaceTarget(GetNextPathPoint());

        if (
            !enemy.agent.pathPending
            && enemy.agent.remainingDistance <= enemy.agent.stoppingDistance
        )
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }
}
