using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeMoveState : EnemyState
{
    private EnemyMelee enemy;

    private Vector3 destination;

    public EnemyMeleeMoveState(
        Enemy enemyBase,
        EnemyStateMachine stateMachine,
        string animatorBoolName
    )
        : base(enemyBase, stateMachine, animatorBoolName)
    {
        enemy = enemyBase as EnemyMelee;
    }

    public override void Enter()
    {
        base.Enter();

        destination = enemy.GetPatrolDestination();
        enemy.agent.SetDestination(destination);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.transform.rotation = enemy.FaceTarget(GetNextPathPoint());

        if (
            !enemy.agent.pathPending
            && enemy.agent.remainingDistance <= enemy.agent.stoppingDistance + 0.5f
        )
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    // To face correctly when go near corner (optional or just use destination)
    private Vector3 GetNextPathPoint()
    {
        NavMeshAgent agent = enemy.agent;

        NavMeshPath path = agent.path;

        if (path.corners.Length < 2)
        {
            return agent.destination;
        }

        for (int i = 0; i < path.corners.Length; i++)
        {
            if (Vector3.Distance(agent.transform.position, path.corners[i]) < 1)
            {
                return path.corners[i + 1];
            }
        }

        return agent.destination;
    }
}
