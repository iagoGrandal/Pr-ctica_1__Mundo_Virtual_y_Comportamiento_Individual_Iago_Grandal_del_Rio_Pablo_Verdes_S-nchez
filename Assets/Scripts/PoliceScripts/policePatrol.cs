using UnityEngine;
using UnityEngine.AI;

public class PolicePatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPointIndex = 0;
    private NavMeshAgent agent;
    private bool isPatrolling = true; // Nueva variable de estado

    public bool IsPatrolling => isPatrolling;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[currentPointIndex].position);
        }
    }

    void Update()
    {
        if (isPatrolling && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            NextPatrolPoint();
        }
    }

    private void NextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPointIndex].position);
    }

    public void PausePatrolling()
    {
        isPatrolling = false;
    }

    public void ResumePatrolling()
    {
        isPatrolling = true;
        NextPatrolPoint();
    }
}
