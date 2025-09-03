using UnityEngine;

public class EnemyMeleeIdleState : EnemyState
{
    private EnemyMelee enemyMelee;

    public EnemyMeleeIdleState(
        Enemy enemyBase,
        EnemyStateMachine stateMachine,
        string animatorBoolName
    )
        : base(enemyBase, stateMachine, animatorBoolName)
    {
        enemyMelee = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemyBase.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemyMelee.moveState);
        }
    }
}
