using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyStateMachine stateMachine { get; private set; }

    [Header("Idle State")]
    public float idleTime;

    [Header("Move State")]
    public float moveTime;

    [SerializeField]
    private Transform[] patrolPoints;

    public int currentPatrolIndex;

    public NavMeshAgent agent { get; private set; }

    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();

        agent = GetComponent<NavMeshAgent>();
    }

    protected virtual void Start()
    {
        InitializePatrolPoint();
    }

    protected virtual void Update() { }

    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPoints[currentPatrolIndex].transform.position;

        currentPatrolIndex++;

        if (currentPatrolIndex >= patrolPoints.Length)
        {
            currentPatrolIndex = 0;
        }

        return destination;
    }

    private void InitializePatrolPoint()
    {
        foreach (Transform t in patrolPoints)
        {
            t.parent = null;
        }
    }
}
