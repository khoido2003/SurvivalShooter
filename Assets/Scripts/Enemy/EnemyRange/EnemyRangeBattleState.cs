using UnityEngine;

public class EnemyRangeBattleState : EnemyState
{
    private EnemyRange enemy;

    private float lastTimeShot = -10f;

    public EnemyRangeBattleState(
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
    }

    public override void Update()
    {
        base.Update();

        enemy.FaceTarget(enemy.player.transform.position);

        if (Time.time > lastTimeShot + 1f / enemy.fireRate)
        {
            enemy.FireSingleBullet();
            lastTimeShot = Time.time;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
