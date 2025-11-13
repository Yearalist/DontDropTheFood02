using System;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
     [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float turnSmoothTime = 0.1f;
    public float jumpHeight = 1.2f;
    public float gravity = -9.81f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.3f;
    public LayerMask groundMask;

    [Header("Camera")]
    public Transform cameraTarget;
    public float topClamp = 70f;
    public float bottomClamp = -30f;

    private CharacterController controller;
    private float turnSmoothVelocity;
    private float verticalVelocity;
    private bool isGrounded;
   

    private float xRotation = 0f;
    private float yRotation = 0f;
    public Animator animator; 


    void Start()
    {
        controller = GetComponent<CharacterController>();
        yRotation = cameraTarget.eulerAngles.y;
    }

    void Update()
    {
        CheckGround();
        HandleCamera();
        HandleMovement();
        HandleJumpAndGravity();
    }

    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
    }

    void HandleCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, bottomClamp, topClamp);

        cameraTarget.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        animator.SetFloat("Speed", direction.magnitude); // animasyona hız gönder

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTarget.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * targetSpeed * Time.deltaTime);
        }
    }

    void HandleJumpAndGravity()
    {
        if (isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;

        if (Input.GetButtonDown("Jump") && isGrounded)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);

        animator.SetBool("IsJumping", !isGrounded); // jump animasyonu
    }

   
}
