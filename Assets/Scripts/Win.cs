using UnityEngine;
using UnityEngine.UI;

public class ObjectInteraction : MonoBehaviour
{
    public GameObject mensajeGanador; // Objeto de texto UI para mostrar el mensaje
    public Camera camaraPrincipal;    // Cámara normal del jugador
    public Camera camaraGanar;        // Cámara que muestra el objeto al ganar
    public GameObject thief;          // Referencia al jugador

    private ThiefInventory thiefInv;  // Inventario del ladrón

    void Start()
    {
        if (thief != null)
        {
            thiefInv = thief.GetComponent<ThiefInventory>(); // Inicializar el inventario
        }
        else
        {
            Debug.LogError("Thief no asignado en el inspector.");
        }

        if (mensajeGanador != null)
        {
            mensajeGanador.SetActive(false); // Oculta el mensaje al inicio
        }

        if (camaraGanar != null)
        {
            camaraGanar.gameObject.SetActive(false); // Desactiva la cámara de victoria al inicio
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Thief")) // Detecta si el jugador toca el objeto
        {
            if (thiefInv != null && thiefInv.HasItem) // Verifica si el jugador tiene el objeto
            {
                if (mensajeGanador != null)
                {
                    mensajeGanador.SetActive(true); // Muestra el mensaje de "¡Ganaste!"
                }

                if (camaraGanar != null && camaraPrincipal != null)
                {
                    camaraPrincipal.gameObject.SetActive(false); // Apaga la cámara principal
                    camaraGanar.gameObject.SetActive(true); // Activa la cámara de victoria
                }
            }
            else
            {
                Debug.Log("No tienes el objeto necesario para ganar.");
            }
        }
    }
}
