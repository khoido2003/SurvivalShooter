using UnityEngine;

public class EnemyMeleeChaseState : EnemyState
{
    private EnemyMelee enemy;
    private float lastTimeUpdateDestination;

    public EnemyMeleeChaseState(
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

        enemy.agent.speed = enemy.chaseSpeed;
        enemy.agent.isStopped = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerInAttackRange())
        {
            stateMachine.ChangeState(enemy.attackState);
        }

        enemy.transform.rotation = enemy.FaceTarget(GetNextPathPoint());

        if (CanUpdateDestination())
        {
            enemy.agent.destination = enemy.player.transform.position;
        }
    }

    private bool CanUpdateDestination()
    {
        if (Time.time > lastTimeUpdateDestination + .25f)
        {
            lastTimeUpdateDestination = Time.time;

            return true;
        }
        return false;
    }
}
