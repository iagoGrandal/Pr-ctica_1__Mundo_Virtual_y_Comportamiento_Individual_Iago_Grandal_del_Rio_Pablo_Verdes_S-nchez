using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefMove : MonoBehaviour
{
    public float runSpeed = 7.0f;
    public float rotationSpeed = 250.0f;
    public float mouseSensitivity = 2.0f;
    public Animator animator;

    private float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor en el centro de la pantalla
        Cursor.visible = false; // Oculta el cursor
    }

    void Update()
    {
        // Movimiento con WASD sin afectar la rotación
        float x = Input.GetAxis("Horizontal"); // Movimiento lateral (A y D)
        float y = Input.GetAxis("Vertical");   // Movimiento adelante/atrás (W y S)

        Vector3 moveDirection = (transform.right * x + transform.forward * y).normalized;
        transform.position += moveDirection * runSpeed * Time.deltaTime;

        // Rotación con el ratón
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY += mouseX * rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, rotationY, 0);

        // Animaciones
        if (animator != null)
        {
            animator.SetFloat("VelX", x);
            animator.SetFloat("VelY", y);
        }
    }
}
