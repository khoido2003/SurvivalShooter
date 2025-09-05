using UnityEngine;

public class EnemyMelee : Enemy
{
    public EnemyMeleeIdleState idleState { get; private set; }

    public EnemyMeleeMoveState moveState { get; private set; }

    public EnemyMeleeRecoveryState recoveryState { get; private set; }

    public EnemyMeleeChaseState chaseState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new EnemyMeleeIdleState(this, stateMachine, "Idle");
        moveState = new EnemyMeleeMoveState(this, stateMachine, "Move");

        recoveryState = new EnemyMeleeRecoveryState(this, stateMachine, "Recovery");

        chaseState = new EnemyMeleeChaseState(this, stateMachine, "Chase");
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
}
