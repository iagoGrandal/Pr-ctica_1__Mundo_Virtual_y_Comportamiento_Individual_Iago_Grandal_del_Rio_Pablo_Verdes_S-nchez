using UnityEngine;
public class PoliceBrain : MonoBehaviour
{
    private PolicePatrol patrol;
    private PoliceVision vision;
    private PoliceChase chase;
    private PoliceSearch search;

    private Vector3 lastKnownPosition;
    private Transform target;

    void Start()
    {
        patrol = GetComponent<PolicePatrol>();
        vision = GetComponent<PoliceVision>();
        chase = GetComponent<PoliceChase>();
        search = GetComponent<PoliceSearch>();

        vision.OnThiefSpotted += HandleThiefSpotted;
        vision.OnThiefLost += HandleThiefLost;
        chase.OnThiefCaught += HandleThiefCaught;
        search.OnSearchComplete += HandleSearchComplete;
    }

    private void HandleThiefSpotted(Transform thief)
    {
        lastKnownPosition = thief.position;
        target = thief;

        patrol?.PausePatrolling();
        chase?.StartChase(thief);
    }

    private void HandleThiefLost()
    {
        chase?.StopChase();
        search?.StartSearch(lastKnownPosition);
    }

    private void HandleSearchComplete()
    {
        patrol?.ResumePatrolling();
    }

    private void HandleThiefCaught()
    {
        if (target != null)
        {
            Destroy(target.gameObject); // Elimina al ladrón de la escena
            target = null; // Evita referencias nulas
        }
    }
}
