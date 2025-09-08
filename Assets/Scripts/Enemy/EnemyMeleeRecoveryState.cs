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
        triggerCalled = false;
    }

    public override void Update()
    {
        base.Update();
        {
            if (enemy.IsPlayerInAttackRange())
            {
                stateMachine.ChangeState(enemy.attackState);
            }
            else if (enemy.IsAxeReady())
            {
                stateMachine.ChangeState(enemy.abilityState);
            }
            else
            {
                stateMachine.ChangeState(enemy.chaseState);
            }
        }
    }
}
