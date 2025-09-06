using System.Collections.Generic;
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

        ChooseRandomNextAttack();
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

    private void ChooseRandomNextAttack()
    {
        // Choose recovery anaimation
        int recoveryIndex = IsPlayerClose() ? 1 : 0;
        enemy.animator.SetFloat("recoveryIndex", recoveryIndex);

        // Choose attack animation
        enemy.attackData = UpdateAttackData();
    }

    private bool IsPlayerClose()
    {
        return Vector3.Distance(enemy.transform.position, enemy.player.transform.position) <= 1;
    }

    private AttackData UpdateAttackData()
    {
        List<AttackData> validAttacks = new(enemy.attackList);

        if (IsPlayerClose())
        {
            validAttacks.RemoveAll(parameter =>
                parameter.attackMeleeType == AttackMeleeType.Charge
            );
        }

        int random = Random.Range(0, validAttacks.Count);
        return validAttacks[random];
    }
}
