using UnityEngine;

public class EnemyMeleeDeadState : EnemyState
{
    private EnemyMelee enemy;
    private EnemyRagdoll enemyRagdoll;

    private bool interactionDisabled = false;

    public EnemyMeleeDeadState(
        Enemy enemyBase,
        EnemyStateMachine stateMachine,
        string animatorBoolName
    )
        : base(enemyBase, stateMachine, animatorBoolName)
    {
        enemy = enemyBase as EnemyMelee;
        enemyRagdoll = enemy.GetComponent<EnemyRagdoll>();
    }

    public override void Enter()
    {
        base.Enter();

        interactionDisabled = false;

        enemy.animator.enabled = false;
        enemy.agent.isStopped = true;

        enemyRagdoll.RagDollActive(true);

        stateTimer = 1.5f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0 && !interactionDisabled)
        {
            interactionDisabled = true;
            enemyRagdoll.RagDollActive(false);
            enemyRagdoll.ColliderActive(false);

            DisableAllColliders();
        }
    }

    private void DisableAllColliders()
    {
        foreach (var col in enemy.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
    }
}
