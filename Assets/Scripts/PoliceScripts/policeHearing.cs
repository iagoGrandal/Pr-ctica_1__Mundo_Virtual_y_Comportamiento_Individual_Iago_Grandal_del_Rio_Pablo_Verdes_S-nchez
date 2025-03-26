using UnityEngine;
using System;

public class PoliceHearing : MonoBehaviour
{
    public Transform thief;
    public float baseHearingRange = 15f;
    public float sprintMultiplier = 2f;
    public float sneakMultiplier = 0.5f;
    public float uncertaintyRadius = 5f;
    public float movementThreshold = 1f;
    public LayerMask thiefLayer;

    public event Action<Vector3> OnSoundHeard;

    private ThiefMove thiefMove;
    private bool isSoundDetected = false;
    private Vector3 lastThiefPosition = Vector3.zero;

    void Start()
    {
        if (thief != null)
        {
            thiefMove = thief.GetComponent<ThiefMove>();
        }
    }

    void Update()
    {
        bool canHear = CanHearThief(out Vector3 soundPosition);

        if (canHear && !isSoundDetected)
        {
            isSoundDetected = true;
            OnSoundHeard?.Invoke(soundPosition);
        }
        else if (canHear && isSoundDetected)
        {
            OnSoundHeard?.Invoke(soundPosition);
        }
        else if (!canHear && isSoundDetected)
        {
            isSoundDetected = false;
        }
    }

    private bool CanHearThief(out Vector3 soundPosition)
    {
        soundPosition = Vector3.zero;
        if (thief == null || thiefMove == null) return false;

        float adjustedHearingRange = baseHearingRange;

        // Modificar el rango de audición según el estado del ladrón
        if (thiefMove.isSprinting)
        {
            adjustedHearingRange *= sprintMultiplier;
        }
        else if (thiefMove.isSneaking)
        {
            adjustedHearingRange *= sneakMultiplier;
        }

        float distanceToThief = Vector3.Distance(transform.position, thief.position);
        float distanceMoved = Vector3.Distance(lastThiefPosition, thief.position);

        if (distanceMoved < movementThreshold)
            return false;


        if (distanceToThief <= adjustedHearingRange)
        {
            lastThiefPosition = thief.position;

            Vector3 offset = UnityEngine.Random.insideUnitSphere * uncertaintyRadius;
            offset.y = 0;
            soundPosition = thief.position + offset;

            return true;
        }

        return false;
    }
}
