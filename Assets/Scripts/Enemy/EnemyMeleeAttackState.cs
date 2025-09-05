using UnityEngine;

public class EnemyMeleeAttackState : EnemyState
{
    private EnemyMelee enemy;
    private Vector3 attackDirection;

    private const float MAX_ATTACK_DISTANCE = 50f;

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

        enemy.PullWeapon();

        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

        attackDirection = enemy.transform.position + enemy.transform.forward * MAX_ATTACK_DISTANCE;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.GetManualMovement())
        {
            enemy.transform.position = Vector3.MoveTowards(
                enemy.transform.position,
                attackDirection,
                enemy.attackMoveSpeed * Time.deltaTime
            );
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.recoveryState);
        }
    }
}
