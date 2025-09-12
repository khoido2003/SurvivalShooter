using UnityEngine;

public class EnemyRange : Enemy
{
    public Transform weaponHolder;

    public EnemyRangeIdleState idleState { get; private set; }
    public EnemyRangeMoveState moveState { get; private set; }
    public EnemyRangeBattleState battleState { get; private set; }

    public float fireRate = 1;

    public Transform gunPoint;
    public float bulletSpeed = 20;
    public int bulletToShoot = 5;
    public float weaponCooldown = 1.5f;

    protected override void Awake()
    {
        base.Awake();

        idleState = new EnemyRangeIdleState(this, stateMachine, "Idle");
        moveState = new EnemyRangeMoveState(this, stateMachine, "Move");
        battleState = new EnemyRangeBattleState(this, stateMachine, "Battle");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public override void EnterBattleMode()
    {
        if (inBattleMode)
        {
            return;
        }

        base.EnterBattleMode();

        stateMachine.ChangeState(battleState);
    }

    public void FireSingleBullet()
    {
        animator.SetTrigger("Shoot");

        Vector3 bulletDirection = (
            player.transform.position + Vector3.up - gunPoint.position
        ).normalized;

        EnemyBullet newBullet = PoolManager.Instance.Get<EnemyBullet>();

        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        newBullet.BulletSetup();

        Rigidbody rbNewBullet = newBullet.GetRigidbody();

        rbNewBullet.mass = 20 / bulletSpeed;
        rbNewBullet.linearVelocity = bulletDirection * bulletSpeed;
    }
}
