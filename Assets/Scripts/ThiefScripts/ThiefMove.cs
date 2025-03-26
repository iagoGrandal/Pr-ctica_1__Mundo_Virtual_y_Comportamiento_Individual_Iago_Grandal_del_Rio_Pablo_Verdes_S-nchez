using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefMove : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float sprintSpeed = 9.0f;
    public float sneakSpeed = 2.5f;
    public float rotationSpeed = 250.0f;
    public float mouseSensitivity = 2.0f;
    public Animator animator;

    [HideInInspector] public bool isSprinting = false;
    [HideInInspector] public bool isSneaking = false;

    private float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Detectar teclas para sprint y sigilo
        isSprinting = Input.GetKey(KeyCode.LeftShift);
        isSneaking = Input.GetKey(KeyCode.LeftControl);

        float currentSpeed = walkSpeed;
        if (isSprinting) currentSpeed = sprintSpeed;
        else if (isSneaking) currentSpeed = sneakSpeed;

        // Movimiento con WASD
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 moveDirection = (transform.right * x + transform.forward * y).normalized;
        transform.position += moveDirection * currentSpeed * Time.deltaTime;

        // Rotación con el ratón
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationY += mouseX * rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, rotationY, 0);

        // Animaciones
        if (animator != null)
        {
            animator.SetFloat("VelX", x);
            animator.SetFloat("VelY", y);
            animator.SetBool("IsSprinting", isSprinting);
            animator.SetBool("IsSneaking", isSneaking);
        }
    }
}
