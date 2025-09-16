using UnityEngine;

public class EnemyRangeBattleState : EnemyState
{
    private EnemyRange enemy;

    private float lastTimeShot = -10f;

    private int bulletShot = 0;

    private int bulletsPerAttack;
    private float weaponCooldown;

    private float coverCheckTimer;

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

        enemy.agent.isStopped = true;
        enemy.agent.velocity = Vector3.zero;

        bulletsPerAttack = enemy.enemyRangeWeaponData.GetBulletsPerAttack();
        weaponCooldown = enemy.enemyRangeWeaponData.GetWeaponCoolDown();

        enemy.enemyVisual.EnableLk(true, true);
    }

    public override void Update()
    {
        base.Update();

        // If player is not in agrreession range => Advance State
        if (!enemy.IsPlayerInAgressionRange())
        {
            stateMachine.ChangeState(enemy.advanceState);
        }

        ShouldChangeCover();

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

        enemy.enemyVisual.EnableLk(false, false);
    }

    #region Weapon Region

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

    #endregion

    #region Cover System


    private void ShouldChangeCover()
    {
        if (enemy.coverPerk != CoverPerk.CanTakeAndChangeCover)
        {
            return;
        }

        coverCheckTimer -= Time.deltaTime;

        if (coverCheckTimer < 0)
        {
            coverCheckTimer = .5f;

            if (IsPlayerInClearSight() || IsPlayerClose())
            {
                if (enemy.CanGetCover())
                {
                    stateMachine.ChangeState(enemy.coverState);
                }
            }
        }
    }

    private bool IsPlayerClose()
    {
        return Vector3.Distance(enemy.transform.position, enemy.player.transform.position)
            < enemy.safeDistance;
    }

    private bool IsPlayerInClearSight()
    {
        Vector3 directionToPlayer = enemy.player.transform.position - enemy.transform.position;

        float distanceToPlayer = directionToPlayer.magnitude;

        directionToPlayer.Normalize();

        float sphereRadius = 0.5f;

        if (
            Physics.SphereCast(
                enemy.transform.position,
                sphereRadius,
                directionToPlayer,
                out RaycastHit hit,
                distanceToPlayer
            )
        )
        {
            if (hit.collider.GetComponentInParent<Player>())
            {
                return true;
            }
        }

        return false;
    }

    #endregion
}
