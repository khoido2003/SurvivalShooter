using UnityEngine;

public class EnemyRangeBattleState : EnemyState
{
    private EnemyRange enemy;

    private float lastTimeShot = -10f;

    private int bulletShot = 0;

    private int bulletsPerAttack;
    private float weaponCooldown;

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

        bulletsPerAttack = enemy.enemyRangeWeaponData.GetBulletsPerAttack();
        weaponCooldown = enemy.enemyRangeWeaponData.GetWeaponCoolDown();

        enemy.enemyVisual.EnableLk(true);
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

        enemy.enemyVisual.EnableLk(false);
    }

    private bool WeaponOnCooldown() => Time.time > lastTimeShot + weaponCooldown;

    private bool WeaponOutOfBullets() => bulletShot >= bulletsPerAttack;

    private void AttemptToReset()
    {
        bulletShot = 0;
        bulletsPerAttack = enemy.enemyRangeWeaponData.GetBulletsPerAttack();
        weaponCooldown = enemy.enemyRangeWeaponData.GetWeaponCoolDown();
    }

    private bool CanShoot()
    {
        return Time.time > lastTimeShot + 1f / enemy.enemyRangeWeaponData.fireRate;
    }

    private void Shoot()
    {
        enemy.FireSingleBullet();
        lastTimeShot = Time.time;

        bulletShot++;
    }
}
