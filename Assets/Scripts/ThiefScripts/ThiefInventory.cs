using UnityEngine;

public class ThiefInventory : MonoBehaviour
{
    private bool hasItem = false; // Indica si el ladrón tiene el objeto o no

    public bool HasItem => hasItem; // Propiedad de solo lectura para verificar si tiene el objeto

    public void PickUpItem()
    {
        hasItem = true;
    }
}
