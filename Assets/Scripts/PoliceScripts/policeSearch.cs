using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;

public class PoliceSearch : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 lastKnownPosition;
    private bool isSearching = false;
    public float searchRotationSpeed = 90f; // Velocidad de giro en grados por segundo
    public float searchDuration = 4f; // Duración total de la búsqueda (giro)

    public event Action OnSearchComplete;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void StartSearch(Vector3 targetPosition)
    {
        if (!isSearching)
        {
            lastKnownPosition = targetPosition;
            StartCoroutine(SearchRoutine());
        }
    }

    private IEnumerator SearchRoutine()
    {
        isSearching = true;
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

        isSearching = false;
        OnSearchComplete?.Invoke();
    }

}
