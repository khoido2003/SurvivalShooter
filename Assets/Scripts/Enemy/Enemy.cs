using System.Collections;
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

    private Vector3[] patrolPointPosition;

    public int currentPatrolIndex;

    public NavMeshAgent agent { get; private set; }

    public Animator animator { get; private set; }

    public Player player;

    [SerializeField]
    protected int healthPoint;

    public bool inBattleMode { get; private set; }

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

    protected virtual void Update()
    {
        if (ShouldEnterBattleMode())
        {
            EnterBattleMode();
        }
    }

    #region Patrol Logic

    public Vector3 GetPatrolDestination()
    {
        Vector3 destination = patrolPointPosition[currentPatrolIndex];

        currentPatrolIndex++;

        if (currentPatrolIndex >= patrolPoints.Length)
        {
            currentPatrolIndex = 0;
        }

        return destination;
    }

    private void InitializePatrolPoint()
    {
        patrolPointPosition = new Vector3[patrolPoints.Length];

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPointPosition[i] = patrolPoints[i].position;
            patrolPoints[i].gameObject.SetActive(false);
        }
    }

    #endregion


    public void FaceTarget(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(target - transform.position);

        Vector3 currentEulerAngles = transform.rotation.eulerAngles;

        float yRotation = Mathf.LerpAngle(
            currentEulerAngles.y,
            targetRotation.eulerAngles.y,
            turnSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.Euler(
            currentEulerAngles.x,
            yRotation,
            currentEulerAngles.z
        );
    }

    public virtual void GetHit()
    {
        EnterBattleMode();
        healthPoint--;
    }

    protected bool ShouldEnterBattleMode()
    {
        bool inAgressionRange =
            Vector3.Distance(transform.position, player.transform.position) < aggressionRange;

        if (inAgressionRange)
        {
            return true;
        }

        return false;
    }

    public virtual void EnterBattleMode()
    {
        inBattleMode = true;
    }

    public virtual void DeadHitImpact(Vector3 force, Vector3 hitPoint, Rigidbody rigidbody)
    {
        StartCoroutine(DeadHitImpactCoroutine(force, hitPoint, rigidbody));
    }

    private IEnumerator DeadHitImpactCoroutine(Vector3 force, Vector3 hitPoint, Rigidbody rigidbody)
    {
        yield return new WaitForSeconds(.1f);

        rigidbody.AddForceAtPosition(force, hitPoint, ForceMode.Impulse);
    }

    public virtual void AbilityTrigger()
    {
        stateMachine.currentState.AbilityTrigger();
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, aggressionRange);
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationTrigger();
}
