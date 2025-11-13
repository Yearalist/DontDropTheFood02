using UnityEngine;

public class ControllerPlayer : MonoBehaviour
{
    
    public float speed = 5f;
    public float sideSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    public float currentMoveSpeed { get; private set; }
    public System.Action OnJump;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isJumping;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleGravityAndJump();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D
        float vertical = Input.GetAxisRaw("Vertical");     // W/S

        Vector3 move = transform.forward * vertical * speed + transform.right * horizontal * sideSpeed;
        controller.Move(move * Time.deltaTime);

        // Güncel hareket hızını hesapla (animasyon için)
        currentMoveSpeed = move.magnitude > 0.1f
            ? (Input.GetKey(KeyCode.LeftShift) ? 2f : 1f)
            : 0f;
    }

    void HandleGravityAndJump()
    {
        if (controller.isGrounded)
        {
            if (isJumping)
            {
                isJumping = false;
            }

            velocity.y = -2f; // zemine yapışık kalması için

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
                isJumping = true;
                OnJump?.Invoke(); // Animasyona haber ver
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
