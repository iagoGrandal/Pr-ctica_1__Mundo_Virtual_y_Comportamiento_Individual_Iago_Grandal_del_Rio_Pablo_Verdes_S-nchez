using UnityEngine;
using System;

public class PoliceVision : MonoBehaviour
{
    public Transform thief;
    public float visionRange = 10f;
    public float visionAngle = 45f;
    public LayerMask thiefLayer;
    public LayerMask stopLayer;
    public float eyeHeight = 1.7f; // Altura desde donde se realiza la detección

    public event Action<Transform> OnThiefSpotted;
    public event Action<Transform> OnThiefLost;

    private bool isThiefVisible = false;
    private Transform currentThief;

    void Update()
    {
        bool canSee = CanSeeThief(out Transform detectedThief);

        if (canSee && !isThiefVisible)
        {
            isThiefVisible = true;
            currentThief   = detectedThief;
            OnThiefSpotted?.Invoke(detectedThief);
        }
        else if (!canSee && isThiefVisible)
        {
            isThiefVisible = false;
            OnThiefLost?.Invoke(currentThief);
            currentThief = null;
        }
    }

    private bool CanSeeThief(out Transform detectedThief)
    {
        detectedThief = null;
        if (thief == null) return false;

        // Se define el origen y la posición del ladrón con el offset de altura
        Vector3 origin    = transform.position + Vector3.up * eyeHeight;
        Vector3 thiefPos  = thief.position   + Vector3.up * eyeHeight;

        Vector3 directionToThief = (thiefPos - origin).normalized;
        float   angleToThief     = Vector3.Angle(transform.forward, directionToThief);

        if (angleToThief < visionAngle / 2f)
        {
            if (Physics.Raycast(origin, directionToThief, out RaycastHit hit, visionRange))
            {
                int layerMask = 1 << hit.transform.gameObject.layer;
                if ((layerMask & stopLayer) != 0)
                    return false;

                if ((layerMask & thiefLayer) != 0 && hit.transform == thief)
                {
                    detectedThief = thief;
                    return true;
                }
            }
        }
        return false;
    }
}
