using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyStateMachine stateMachine { get; private set; }

    public float turnSpped;

    public float aggressionRange;

    [Header("Idle State")]
    public float idleTime;

    [Header("Move State")]
    public float moveSpeed;

    [Header("Chase State")]
    public float chaseSpeed;

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
            turnSpped * Time.deltaTime
        );

        return Quaternion.Euler(currentEulerAngles.x, yRotation, currentEulerAngles.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggressionRange);
    }

    public bool IsPlayerInAggressionRange()
    {
        return Vector3.Distance(transform.position, player.transform.position) < aggressionRange;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
}
