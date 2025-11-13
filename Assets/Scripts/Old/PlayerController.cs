using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float sidespeed;
    public float jumpForce;
    public bool isGrounded;
    
    public float groundCheckDistance = 0.3f;
    public LayerMask groundLayer;

    public Rigidbody hips;

    private Vector3 moveDirection;
    public float currentMoveSpeed { get; private set; } // animasyon scripti bunu okuyacak

    public System.Action OnJump; // animasyon scripti bunu dinleyebilir

    private void Start()
    {
        hips = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        moveDirection = Vector3.zero;
        currentMoveSpeed = 0f;
        CheckGrounded();

        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveDirection += hips.transform.forward * speed * 1.5f;
                currentMoveSpeed = 2f;
            }
            else
            {
                moveDirection += hips.transform.forward * speed;
                currentMoveSpeed = 1f;
            }
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += -hips.transform.right * sidespeed;
            currentMoveSpeed = Mathf.Max(currentMoveSpeed, 1f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += -hips.transform.forward * speed;
            currentMoveSpeed = Mathf.Max(currentMoveSpeed, 1f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += hips.transform.right * sidespeed;
            currentMoveSpeed = Mathf.Max(currentMoveSpeed, 1f);
        }

        hips.AddForce(moveDirection);

        if (Input.GetAxis("Jump") > 0 && isGrounded)
        {
            hips.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

            // Animasyon scriptine haber ver
            OnJump?.Invoke();
        }
        
        void CheckGrounded()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            isGrounded = Physics.Raycast(ray, 1.1f); //
        }
    }
    
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
   
   
}
