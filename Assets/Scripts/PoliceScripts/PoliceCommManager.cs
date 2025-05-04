// PoliceCommManager.cs
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PoliceCommManager : MonoBehaviour
{
    public static PoliceCommManager Instance;

    [Header("Puntos clave")]
    [Tooltip("Posición de la puerta de salida")]
    public Vector3 exitPosition;
    [Tooltip("Posición del tesoro que protegen")]
    public Vector3 treasurePosition;

    [Tooltip("Tiempo (s) para recopilar informes de posición")]
    public float reportTimeout = 0.2f;

    /// <summary>Evento que lanza el policía que ve al ladrón: (quién, dónde).</summary>
    public event Action<Transform, Vector3> OnThiefSpotted;

    // Estructura para acumular cada informe
    struct Report { public Transform agent; public float distExit, distTreasure; }
    List<Report> reports = new List<Report>();

    // Guarda la última posición conocida del ladrón
    private Vector3 lastThiefPosition;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Llamar desde PoliceBrain cuando un agente ve al ladrón.
    /// </summary>
    public void BroadcastThiefSpotted(Transform spy, Vector3 thiefPos)
    {
        Debug.Log($"Ladrón visto por {spy.name} en {thiefPos}");
        // Reiniciamos los informes
        reports.Clear();
        lastThiefPosition = thiefPos;

        // Avisamos a todos los suscritos
        OnThiefSpotted?.Invoke(spy, thiefPos);

        // Recogemos posiciones durante un breve intervalo
        StartCoroutine(CollectReports());
    }

    /// <summary>
    /// Llamar desde cada agente que NO ve al ladrón para reportar su posición.
    /// </summary>
    public void BroadcastPosition(Transform agent, Vector3 agentPos)
    {
        Debug.Log($"{agent.name} reporta su posición: {agentPos}");
        reports.Add(new Report {
            agent        = agent,
            distExit     = Vector3.Distance(agentPos, exitPosition),
            distTreasure = Vector3.Distance(agentPos, treasurePosition)
        });
    }

    IEnumerator CollectReports()
    {
        yield return new WaitForSeconds(reportTimeout);
        AssignRoles();
    }

    /// <summary>
    /// Tras el timeout, elige el más cercano a la puerta y al tesoro.
    /// </summary>
    void AssignRoles()
    {
        if (reports.Count == 0) return;

        // Quién irá a la salida
        var nearestExit     = reports.OrderBy(r => r.distExit)    .First().agent;
        Debug.Log($"Policía más cercano a la salida: {nearestExit.name}");
        // Eliminamos a nearestExit de los informes para evitar duplicados
        reports = reports.Where(r => r.agent != nearestExit).ToList();
        // Quién irá al tesoro
        var nearestTreasure = reports.OrderBy(r => r.distTreasure).First().agent;
        Debug.Log($"Policía más cercano al tesoro: {nearestExit.name}");
        reports = reports.Where(r => r.agent != nearestTreasure).ToList();



        // Órdenes
        nearestExit
            .GetComponent<PoliceBrain>()
            .GoToExit(exitPosition);

        nearestTreasure
            .GetComponent<PoliceBrain>()
            .GoToTreasureOrChase(treasurePosition);
        
        foreach (var report in reports)
        {
            report.agent
                .GetComponent<PoliceBrain>()
                .HandleSoundFound(lastThiefPosition);
        }
    }
}
