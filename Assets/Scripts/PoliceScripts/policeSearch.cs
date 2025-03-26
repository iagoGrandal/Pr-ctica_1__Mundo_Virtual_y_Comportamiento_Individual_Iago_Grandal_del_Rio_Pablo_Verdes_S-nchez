using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;

public class PoliceSearch : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 lastKnownPosition;
    public float searchRotationSpeed = 90f; // Velocidad de giro en grados por segundo
    public float searchDuration = 4f; // Duración total de la búsqueda (giro)

    public event Action OnSearchComplete;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void StartSearch(Vector3 targetPosition)
    {
        lastKnownPosition = targetPosition;
        Debug.Log($"{lastKnownPosition}");
        StartCoroutine(SearchRoutine());
    }

    private IEnumerator SearchRoutine()
    {
        agent.isStopped = false;
        agent.SetDestination(lastKnownPosition);

        while (agent.pathPending || agent.remainingDistance > 0.2f) 
        {
            yield return null;
        }


        float elapsedTime = 0f;
        while (elapsedTime < searchDuration)
        {
            transform.Rotate(Vector3.up, searchRotationSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        OnSearchComplete?.Invoke();
    }

}
