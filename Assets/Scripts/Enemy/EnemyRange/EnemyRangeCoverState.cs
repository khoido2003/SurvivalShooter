using UnityEngine;

public class EnemyRangeCoverState : EnemyState
{
    private EnemyRange enemy;

    private Vector3 destination;

    public EnemyRangeCoverState(
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

        enemy.enemyVisual.EnableLk(true, false);
        enemy.agent.isStopped = false;

        enemy.agent.speed = enemy.runSpeed;

        destination = enemy.lastCover.position;
        enemy.agent.SetDestination(destination);
    }

    public override void Update()
    {
        base.Update();

        enemy.FaceTarget(GetNextPathPoint());

        if (Vector3.Distance(enemy.transform.position, destination) < .5f)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
