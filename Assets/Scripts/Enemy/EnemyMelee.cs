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

public enum EnemyMeleeType
{
    Regular,
    Shield,
    Dodge,
    AxeThrow,
}

public class EnemyMelee : Enemy
{
    public EnemyMeleeIdleState idleState { get; private set; }

    public EnemyMeleeMoveState moveState { get; private set; }

    public EnemyMeleeRecoveryState recoveryState { get; private set; }

    public EnemyMeleeChaseState chaseState { get; private set; }

    public EnemyMeleeAttackState attackState { get; private set; }

    public EnemyMeleeDeadState deadState { get; private set; }

    public EnemyMeleeAbilityState abilityState { get; private set; }

    public float dogdeCooldown;
    public float lastDodgeTime;

    [Header("Attack data")]
    public AttackData attackData;
    public List<AttackData> attackList;

    [SerializeField]
    private Transform hiddenWeapon;

    [SerializeField]
    private Transform pulledWeapon;

    [Header("Enemy Settings")]
    public EnemyMeleeType enemyMeleeType;

    [SerializeField]
    private Transform shieldTransform;

    [Header("Axe throw ability")]
    public float axeFlySpeed;
    public float axeAimTimer;
    public float axeThrowCooldown;
    public Transform axeStartPoint;
    private float lastTimeAxeThrow;

    protected override void Awake()
    {
        base.Awake();

        idleState = new EnemyMeleeIdleState(this, stateMachine, "Idle");

        moveState = new EnemyMeleeMoveState(this, stateMachine, "Move");

        recoveryState = new EnemyMeleeRecoveryState(this, stateMachine, "Recovery");

        chaseState = new EnemyMeleeChaseState(this, stateMachine, "Chase");

        attackState = new EnemyMeleeAttackState(this, stateMachine, "Attack");

        deadState = new EnemyMeleeDeadState(this, stateMachine, "Idle");

        abilityState = new EnemyMeleeAbilityState(this, stateMachine, "AxeThrow");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);

        InitializeSpeciality();
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public override void GetHit()
    {
        base.GetHit();

        if (healthPoint <= 0)
        {
            stateMachine.ChangeState(deadState);
        }
    }

    public void PullWeapon()
    {
        hiddenWeapon.gameObject.SetActive(false);
        pulledWeapon.gameObject.SetActive(true);
    }

    public bool IsPlayerInAttackRange() =>
        Vector3.Distance(transform.position, player.transform.position) < attackData.attackRange;

    public Transform GetShieldTransform()
    {
        return shieldTransform;
    }

    public void TriggerAbility() { }

    public override void AbilityTrigger()
    {
        base.AbilityTrigger();

        pulledWeapon.gameObject.SetActive(false);
    }

    // Dodge Ability
    public void ActivateDodgeRoll()
    {
        if (enemyMeleeType != EnemyMeleeType.Dodge)
        {
            return;
        }

        if (stateMachine.currentState != chaseState)
        {
            return;
        }

        if (Vector3.Distance(transform.position, player.transform.position) < 1.8f)
        {
            return;
        }

        if (Time.time > dogdeCooldown + lastDodgeTime)
        {
            animator.SetTrigger("dodgeRoll");
            lastDodgeTime = Time.time;
        }
    }

    // Shield Ability
    private void InitializeSpeciality()
    {
        if (enemyMeleeType == EnemyMeleeType.Shield)
        {
            animator.SetFloat("chaseIndex", 1);
            shieldTransform.gameObject.SetActive(true);
        }
    }

    // Axe Throw Ability
    public bool IsAxeReady()
    {
        if (enemyMeleeType != EnemyMeleeType.AxeThrow)
        {
            return false;
        }

        return Time.time > axeThrowCooldown + lastTimeAxeThrow;
    }

    public void ConsumeAxeThrow()
    {
        lastTimeAxeThrow = Time.time;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, attackData.attackRange);
    }
}
