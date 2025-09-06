using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyStateMachine stateMachine { get; private set; }

    [Header("Idle State")]
    public float idleTime;
    public float aggressionRange;

    [Header("Move State")]
    public float moveSpeed;
    private bool manualMovement;
    private float manualRotation;

    [Header("Chase State")]
    public float chaseSpeed;
    public float turnSpeed;

    [SerializeField]
    private Transform[] patrolPoints;

    public int currentPatrolIndex;

    public NavMeshAgent agent { get; private set; }

    public Animator animator { get; private set; }

    public Player player;

    protected virtual void Awake()
    {
        stateMachine = new EnemyStateMachine();

        agent = GetComponent<NavMeshAgent>();

        animator = GetComponentInChildren<Animator>();
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

    public Quaternion FaceTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        Vector3 currentEulerAngles = transform.rotation.eulerAngles;

        float yRotation = Mathf.LerpAngle(
            currentEulerAngles.y,
            targetRotation.eulerAngles.y,
            turnSpeed * Time.deltaTime
        );

        return Quaternion.Euler(currentEulerAngles.x, yRotation, currentEulerAngles.z);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggressionRange);
    }

    public bool IsPlayerInAggressionRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < aggressionRange;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
}
