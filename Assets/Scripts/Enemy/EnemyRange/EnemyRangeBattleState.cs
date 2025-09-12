using UnityEngine;

public class EnemyRangeBattleState : EnemyState
{
    private EnemyRange enemy;

    private float lastTimeShot = -10f;

    private int bulletShot = 0;

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

        if (WeaponOutOfBullets())
        {
            if (WeaponOnCooldown())
            {
                AttemptToReset();
            }
            return;
        }

        if (CanShoot())
        {
            Shoot();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool WeaponOnCooldown() => Time.time > lastTimeShot + enemy.weaponCooldown;

    private bool WeaponOutOfBullets() => bulletShot >= enemy.bulletToShoot;

    private void AttemptToReset()
    {
        bulletShot = 0;
    }

    private bool CanShoot()
    {
        return Time.time > lastTimeShot + 1f / enemy.fireRate;
    }

    private void Shoot()
    {
        enemy.FireSingleBullet();
        lastTimeShot = Time.time;

        bulletShot++;
    }
}
