using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : Enemy
{
    [Header("Weapon Details")]
    public EnemyRangeWeaponModelType weaponModelType;
    public EnemyRangeWeaponData enemyRangeWeaponData;

    public EnemyRangeIdleState idleState { get; private set; }
    public EnemyRangeMoveState moveState { get; private set; }
    public EnemyRangeBattleState battleState { get; private set; }

    public Transform weaponHolder;
    public Transform gunPoint;

    [SerializeField]
    List<EnemyRangeWeaponData> availableWeaponData;

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

        enemyVisual.SetupLook();

        SetupWeaponData();
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

        Vector3 bulletWithSpreadDirection = enemyRangeWeaponData.ApplySpread(bulletDirection);

        rbNewBullet.mass = 20 / enemyRangeWeaponData.bulletSpeed;
        rbNewBullet.linearVelocity = bulletWithSpreadDirection * enemyRangeWeaponData.bulletSpeed;
    }

    private void SetupWeaponData()
    {
        List<EnemyRangeWeaponData> filteredData = new();

        foreach (var weaponData in availableWeaponData)
        {
            if (weaponData.weaponModelType == weaponModelType)
            {
                filteredData.Add(weaponData);
            }
        }

        if (filteredData.Count <= 0)
        {
            Debug.LogError("No Enemy Range Weapon Data available!");
            return;
        }

        int random = Random.Range(0, filteredData.Count);
        enemyRangeWeaponData = filteredData[random];

        gunPoint = enemyVisual.currentWeaponModel.GetComponent<EnemyRangeWeaponModel>().gunPoint;
    }
}
