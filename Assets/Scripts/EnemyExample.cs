using UnityEngine;
using UnityEngine.AI;

public class EnemyExample : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;

    public static EnemyExample Instance { get; private set; }

    private void Update()
    {
        agent.destination = target.position;
    }
}
