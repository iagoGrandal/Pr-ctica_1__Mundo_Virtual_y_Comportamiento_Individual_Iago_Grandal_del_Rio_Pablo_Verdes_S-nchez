using UnityEngine;
public class PoliceBrain : MonoBehaviour
{
    private PolicePatrol patrol;
    private PoliceVision vision;
    private PoliceChase chase;
    private PoliceSearch search;
    private PoliceHearing hearing;


    private Vector3 lastKnownPosition;
    private Transform target;
    private bool isSpotting;

    void Start()
    {
        patrol = GetComponent<PolicePatrol>();
        vision = GetComponent<PoliceVision>();
        chase = GetComponent<PoliceChase>();
        search = GetComponent<PoliceSearch>();
        hearing = GetComponent<PoliceHearing>();

        isSpotting = false;

        vision.OnThiefSpotted += HandleThiefSpotted;
        vision.OnThiefLost += HandleThiefLost;
        chase.OnThiefCaught += HandleThiefCaught;
        search.OnSearchComplete += HandleSearchComplete;
        hearing.OnSoundHeard += HandleSoundFound;
    }

    private void HandleThiefSpotted(Transform thief)
    {
        lastKnownPosition = thief.position;
        target = thief;
        Debug.Log($"El policía te ha visto");
        isSpotting = true;

        patrol?.PausePatrolling();
        chase?.StartChase(thief);
    }

    private void HandleThiefLost()
    {
        chase?.StopChase();
        search?.StartSearch(lastKnownPosition);
        isSpotting = false;
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

    private void HandleSoundFound(Vector3 soundPosition)
    {
        if (!isSpotting)
        {
            Debug.Log($"🕵️‍♂️ Sonido detectado, investigando en: {soundPosition}");
            Debug.Log($"🔍 thief.position actual: {vision.thief.position}");
            search?.StartSearch(soundPosition);
        }
    }
}
