using UnityEngine;

public class EnemyMeleeAbilityState : EnemyState
{
    private EnemyMelee enemy;

    public EnemyMeleeAbilityState(
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

        triggerCalled = false;

        enemy.PullWeapon();
    }

    public override void Update()
    {
        base.Update();

        Vector3 direction = (enemy.player.transform.position - enemy.transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            enemy.transform.rotation = Quaternion.LookRotation(direction);
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.recoveryState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        triggerCalled = false;
        enemy.animator.SetFloat("recoveryIndex", 0);
        enemy.ConsumeAxeThrow();
    }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        // Create thee axe and throw at the player
        EnemyAxe enemyAxe = PoolManager.Instance.Get<EnemyAxe>();

        enemyAxe.transform.position = enemy.axeStartPoint.position;

        enemyAxe.AxeSetup(enemy.axeFlySpeed, enemy.player.transform, enemy.axeAimTimer);
    }
}
