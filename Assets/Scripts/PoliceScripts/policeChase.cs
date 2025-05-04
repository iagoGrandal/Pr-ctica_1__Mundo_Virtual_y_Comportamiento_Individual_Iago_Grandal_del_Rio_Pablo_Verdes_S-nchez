using UnityEngine;
using UnityEngine.AI;
using System;

public class PoliceChase : MonoBehaviour
{
    public float chaseSpeed = 5f;
    public event Action OnThiefCaught;

    private NavMeshAgent agent;
    private Transform target;
    private bool isChasing = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!isChasing || target == null) return;
        agent.SetDestination(target.position);

        if (Vector3.Distance(transform.position, target.position) < 1.5f)
        {
            isChasing = false;
            agent.ResetPath();
            OnThiefCaught?.Invoke();
        }
    }

    public void StartChase(Transform thief)
    {
        target     = thief;
        isChasing  = true;
        agent.speed = chaseSpeed;
        agent.isStopped = false;
    }

    public void StopChase()
    {
        isChasing = false;
        agent.ResetPath();
        agent.isStopped = true;
        target = null;
    }
}
