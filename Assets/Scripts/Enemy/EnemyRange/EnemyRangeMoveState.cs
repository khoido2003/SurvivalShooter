using UnityEngine;

public class EnemyRangeMoveState : EnemyState
{
    private EnemyRange enemy;
    private Vector3 destination;

    public EnemyRangeMoveState(
        Enemy enemyBase,
        EnemyStateMachine stateMachine,
        string animatorBoolName
    )
        : base(enemyBase, stateMachine, animatorBoolName)
    {
        enemy = enemyBase as EnemyRange;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.speed = enemy.moveSpeed;

        destination = enemy.GetPatrolDestination();
        enemy.agent.SetDestination(destination);
    }

    public override void Update()
    {
        base.Update();

        enemy.FaceTarget(GetNextPathPoint());

        if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance + 0.05f)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
