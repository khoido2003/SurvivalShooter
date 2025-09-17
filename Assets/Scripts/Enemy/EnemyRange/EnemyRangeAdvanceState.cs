using UnityEngine;

public class EnemyRangeAdvanceState : EnemyState
{
    private EnemyRange enemy;
    private Vector3 playerPos;

    public float lastTimeAdvanced;

    public EnemyRangeAdvanceState(
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

        enemy.enemyVisual.EnableLk(true, true);

        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.advanceSpeed;

        if (enemy.IsUnstoppable())
        {
            enemy.enemyVisual.EnableLk(true, false);

            stateTimer = enemy.advanceTime;
        }
    }

    public override void Update()
    {
        base.Update();

        playerPos = enemy.player.transform.position;

        enemy.agent.SetDestination(playerPos);
        enemy.FaceTarget(GetNextPathPoint());

        if (CanEnterBattleState())
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        lastTimeAdvanced = Time.time;
    }

    private bool CanEnterBattleState()
    {
        bool closeEnoughToPlayer =
            Vector3.Distance(enemy.transform.position, playerPos) < enemy.advanceStoppingDistance;

        if (enemy.IsUnstoppable())
        {
            return closeEnoughToPlayer || stateTimer < 0;
        }

        return closeEnoughToPlayer;
    }
}
