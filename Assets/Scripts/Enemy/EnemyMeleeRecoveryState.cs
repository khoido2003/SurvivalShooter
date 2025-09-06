using UnityEngine;

public class EnemyMeleeRecoveryState : EnemyState
{
    private EnemyMelee enemy;

    public EnemyMeleeRecoveryState(
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

        enemy.agent.isStopped = true;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.transform.rotation = enemy.FaceTarget(enemy.player.transform.position);

        if (triggerCalled)
        {
            if (enemy.IsPlayerInAttackRange())
            {
                stateMachine.ChangeState(enemy.attackState);
            }
            else
            {
                stateMachine.ChangeState(enemy.chaseState);
            }
        }
    }
}
