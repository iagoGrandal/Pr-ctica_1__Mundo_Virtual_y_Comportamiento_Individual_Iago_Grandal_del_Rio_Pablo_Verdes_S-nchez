using UnityEngine;
using UnityEngine.AI;
using System;

public class PoliceChase : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform target;
    private bool isChasing = false;
    private float captureDistance = 1.5f;

    public bool IsChasing => isChasing;
    public event Action OnThiefCaught;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isChasing && target != null)
        {
            // Actualizamos el destino cada frame para seguir al ladrón
            agent.SetDestination(target.position);

            if (Vector3.Distance(transform.position, target.position) < captureDistance)
            {
                OnThiefCaught?.Invoke();
                StopChase();
            }
        }
    }

    public void StartChase(Transform thief)
    {
        if (!isChasing || target == null)
        {
            target = thief;
            isChasing = true;
            agent.SetDestination(target.position);
        }
    }

    public void StopChase()
    {
        isChasing = false;  
    }
}
