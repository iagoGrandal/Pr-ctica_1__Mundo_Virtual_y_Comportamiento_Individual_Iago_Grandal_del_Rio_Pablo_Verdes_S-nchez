using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PoliceBrain : MonoBehaviour
{
    // --- Componentes existentes ---
    private PolicePatrol   patrol;
    private PoliceVision   vision;
    private PoliceChase    chase;
    private PoliceSearch   search;
    private PoliceHearing  hearing;

    // --- Para movernos con NavMeshAgent ---
    private NavMeshAgent   agent;
    // El gestor de comunicaciones
    private PoliceCommManager comm;

    private Vector3        lastKnownPosition;
    private Transform      target;
    private bool           isSpotting = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        patrol  = GetComponent<PolicePatrol>();
        vision  = GetComponent<PoliceVision>();
        chase   = GetComponent<PoliceChase>();
        search  = GetComponent<PoliceSearch>();
        hearing = GetComponent<PoliceHearing>();

        comm = PoliceCommManager.Instance;
        // Me suscribo al evento global de aviso
        comm.OnThiefSpotted += OnAllySpottedThief;

        // Suscripciones originales de visión, persecución, etc.
        vision.OnThiefSpotted += HandleThiefSpotted;
        vision.OnThiefLost    += HandleThiefLost;
        chase.OnThiefCaught   += HandleThiefCaught;
        search.OnSearchComplete   += HandleSearchComplete;
        hearing.OnSoundHeard  += HandleSoundFound;
    }
    
    // 1) Yo mismo veo al ladrón
    private void HandleThiefSpotted(Transform thief)
    {
        lastKnownPosition = thief.position;
        target            = thief;
        isSpotting        = true;

        patrol?.PausePatrolling();
        chase?.StartChase(thief);

        // Aviso al resto de policías mediante el manager
        comm.BroadcastThiefSpotted(this.transform, lastKnownPosition);
    }

    // 2) Pierdo de vista
    private void HandleThiefLost(Transform thief)
    {
        chase?.StopChase();
        search?.StartSearch(thief.position);
        isSpotting = false;
    }

    // 3) Otro policía avisa, y yo no veo al ladrón
    private void OnAllySpottedThief(Transform spy, Vector3 thiefPos)
    {
        // Ignoro mi propio mensaje y si ya estoy persiguiendo
        if (spy == transform || isSpotting) return;
        // Reporto mi posición al manager
        comm.BroadcastPosition(transform, transform.position);
    }

    // 4) El manager me ordena ir a la puerta
    public void GoToExit(Vector3 exitPos)
    {
        patrol?.PausePatrolling();
        chase?.StopChase();
        Debug.Log($"Voy a la salida: {exitPos}");
        agent.isStopped = false;
        agent.SetDestination(exitPos);
    }

    // 5) El manager me ordena ir al tesoro y, luego, quizá perseguir
    public void GoToTreasureOrChase(Vector3 treasurePos)
    {
        patrol?.PausePatrolling();
        chase?.StopChase();
        Debug.Log($"Voy al tesoro: {treasurePos}");
        agent.isStopped = false;
        agent.SetDestination(treasurePos);
    }

    // --- Resto de handlers sin cambios ---
    private void HandleSearchComplete()
    {
        patrol?.ResumePatrolling();
    }

    private void HandleThiefCaught()
    {
        if (target != null)
        {
            Destroy(target.gameObject);
            target = null;
        }
    }

    public void HandleSoundFound(Vector3 soundPosition)
    {
        if (!isSpotting)
        {
            search?.StartSearch(soundPosition);
        }
    }
}