using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject doorClosed, doorOpened;
    public float openTime;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Thief"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                doorClosed.SetActive(false);
                doorOpened.SetActive(true);
                StartCoroutine(closeDoor());
            }
        }
        else if (other.CompareTag("Police")) 
        {
            doorClosed.SetActive(false);
            doorOpened.SetActive(true);
            StartCoroutine(closeDoor());
        }
    }

    IEnumerator closeDoor()
    {
        yield return new WaitForSeconds(openTime);
        doorOpened.SetActive(false);
        doorClosed.SetActive(true);
    }
}