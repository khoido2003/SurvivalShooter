using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AttackData
{
    public string attackName;
    public float attackRange;
    public float moveSpeed;
    public float attackIndex;

    [Range(1, 2)]
    public float animationSpeed;
    public AttackMeleeType attackMeleeType;
}

public enum AttackMeleeType
{
    Close,
    Charge,
}

public class EnemyMelee : Enemy
{
    public EnemyMeleeIdleState idleState { get; private set; }

    public EnemyMeleeMoveState moveState { get; private set; }

    public EnemyMeleeRecoveryState recoveryState { get; private set; }

    public EnemyMeleeChaseState chaseState { get; private set; }

    public EnemyMeleeAttackState attackState { get; private set; }

    [Header("Attack data")]
    public AttackData attackData;
    public List<AttackData> attackList;

    [SerializeField]
    private Transform hiddenWeapon;

    [SerializeField]
    private Transform pulledWeapon;

    protected override void Awake()
    {
        base.Awake();

        idleState = new EnemyMeleeIdleState(this, stateMachine, "Idle");

        moveState = new EnemyMeleeMoveState(this, stateMachine, "Move");

        recoveryState = new EnemyMeleeRecoveryState(this, stateMachine, "Recovery");

        chaseState = new EnemyMeleeChaseState(this, stateMachine, "Chase");

        attackState = new EnemyMeleeAttackState(this, stateMachine, "Attack");
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

    public void PullWeapon()
    {
        hiddenWeapon.gameObject.SetActive(false);
        pulledWeapon.gameObject.SetActive(true);
    }

    public bool IsPlayerInAttackRange() =>
        Vector3.Distance(transform.position, player.transform.position) < attackData.attackRange;

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, attackData.attackRange);
    }
}
