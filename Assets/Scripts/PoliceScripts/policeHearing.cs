using UnityEngine;
using System;

public class PoliceHearing : MonoBehaviour
{
    public Transform thief;
    public float hearingRange = 15f; // Rango de detección de sonido
    public float uncertaintyRadius = 5f; // Radio de incertidumbre del sonido
    public float movementThreshold = 1f; // Umbral para ignorar movimientos pequeños
    public LayerMask thiefLayer;

    public event Action<Vector3> OnSoundHeard; // Devuelve una posición aproximada
    public event Action OnSoundLost; // Evento cuando se deja de oír el sonido

    private bool isSoundDetected = false;
    private bool isInvestigating = false; // Bloqueo de audición mientras investiga
    private Vector3 lastThiefPosition = Vector3.zero; // Última posición conocida del ladrón

    void Update()
    {
        if (!isInvestigating) // Solo escucha si no está investigando
        {
            bool canHear = CanHearThief(out Vector3 soundPosition);

            if (canHear && !isSoundDetected)
            {
                isSoundDetected = true;
                isInvestigating = true; // Bloquear audición hasta que termine la investigación
                OnSoundHeard?.Invoke(soundPosition);
            }
            else if (!canHear && isSoundDetected)
            {
                isSoundDetected = false;
                OnSoundLost?.Invoke();
            }
        }
    }

    private bool CanHearThief(out Vector3 soundPosition)
    {
        soundPosition = Vector3.zero;
        if (thief == null) return false;

        float distanceToThief = Vector3.Distance(transform.position, thief.position);
        float distanceMoved = Vector3.Distance(lastThiefPosition, thief.position); // Distancia recorrida por el ladrón

        // Si el ladrón no se ha movido lo suficiente, no activamos el sonido
        if (distanceMoved < movementThreshold)
        {
            return false;
        }

        if (distanceToThief < hearingRange)
        {
            // Guardamos la nueva posición del ladrón
            lastThiefPosition = thief.position;

            // Generamos una posición aleatoria dentro del radio de incertidumbre
            Vector3 offset = UnityEngine.Random.insideUnitCircle * uncertaintyRadius;
            soundPosition = thief.position + new Vector3(offset.x, 0, offset.y);

            return true;
        }

        return false;
    }

    // Método para reactivar la audición después de investigar
    public void ResumeHearing()
    {
        isInvestigating = false;
    }
}
