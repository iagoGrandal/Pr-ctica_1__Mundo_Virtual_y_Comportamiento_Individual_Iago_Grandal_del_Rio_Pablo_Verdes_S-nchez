using UnityEngine;

public class StealableItem : MonoBehaviour
{
    private bool isStolen = false;
    public GameObject thief; // Referencia al ladrón (así se asigna desde el inspector)
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
    }

    void OnTriggerEnter(Collider other)
    {

        if (!isStolen && other.CompareTag("Thief")) // Verifica que el ladrón tocó el objeto
        {
            if (thiefInv != null)
            {
                isStolen = true;

                // Marcar en el inventario del ladrón que tiene el objeto
                thiefInv.PickUpItem();

                gameObject.SetActive(false);

            }
            else
            {
                Debug.LogError("ThiefInventory no encontrado en el ladrón.");
            }
        }
    }
}
