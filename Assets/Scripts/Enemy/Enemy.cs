using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStateMachine stateMachine { get; private set; }

    [Header("Idle State Info")]
    public float idleTime;

    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();
    }

    protected virtual void Start() { }

    protected virtual void Update() { }
}
