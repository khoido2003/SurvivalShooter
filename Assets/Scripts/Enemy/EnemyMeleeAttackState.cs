using UnityEngine;

public class EnemyMeleeAttackState : EnemyState
{
    private EnemyMelee enemy;

    private float attackMoveSpeed;

    public EnemyMeleeAttackState(
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

        attackMoveSpeed = enemy.attackData.moveSpeed;
        enemy.animator.SetFloat("attackSpeed", enemy.attackData.animationSpeed);
        enemy.animator.SetFloat("attackIndex", enemy.attackData.attackIndex);

        enemy.PullWeapon();

        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;
    }

    public override void Exit()
    {
        base.Exit();

        enemy.animator.SetFloat("recoveryIndex", 0);

        if (enemy.IsPlayerInAttackRange())
        {
            enemy.animator.SetFloat("recoveryIndex", 1);
        }
    }

    public override void Update()
    {
        base.Update();

        enemy.transform.rotation = enemy.FaceTarget(enemy.player.transform.position);

        if (triggerCalled)
        {
            if (enemy.IsPlayerInAttackRange())
            {
                stateMachine.ChangeState(enemy.recoveryState);
            }
            else
            {
                stateMachine.ChangeState(enemy.chaseState);
            }
        }
    }
}
