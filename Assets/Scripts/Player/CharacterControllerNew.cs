using System;
using System.Security.Cryptography.X509Certificates;
using JetBrains.Annotations;
using Unity.Netcode;
using UnityEngine;

public class CharacterControllerNew : NetworkBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    private float verticalVelocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    public Transform cameraTarget;
    public float topClamp = 70f;
    public float bottomClamp = -30f;
    private float xRotation;
    private float yRotation;
   
    private CharacterController controller;
    [CanBeNull] public Animator _animator;
    
    // animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    
    private bool _hasAnimator;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        yRotation = cameraTarget.eulerAngles.y;
        _hasAnimator = TryGetComponent(out _animator);

        AssignAnimationIDs();

        // Animasyon başlangıç değerleri
        if (_hasAnimator)
        {
            _animator.SetFloat(_animIDSpeed, 1f);
            _animator.SetFloat(_animIDMotionSpeed, 1f);
            _animator.SetBool(_animIDGrounded, true);
            _animator.SetBool(_animIDJump, false);
            _animator.SetBool(_animIDFreeFall, false);
        }
    }



    private void AssignAnimationIDs()
    {
        
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }

    private void Update()
    {

        if (!IsOwner) 
        {
            return;
        }
        GroundCheck();
        HandleCamera();
       // HandleMovement();
        HandleJump();
        _hasAnimator = TryGetComponent(out _animator);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, isGrounded);
            _animator.SetBool(_animIDFreeFall, !isGrounded);
        }
    }


    void HandleCamera()
    {
        
        
        if (!IsOwner) 
        {
            return;
        }
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, bottomClamp, topClamp);

        cameraTarget.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

   

    void HandleMovement()
    {
        
        
        if (!IsOwner) 
        {
            return;
        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isMoving = direction.magnitude >= 0.1f;
        float inputMagnitude = Mathf.Clamp01(direction.magnitude);

        float speed = isRunning ? runSpeed : walkSpeed;

        // Animasyonlar: Hız ve hareket parametrelerini güncelle
        if (_hasAnimator)
        {
            float targetSpeed = isMoving ? (isRunning ? 1f : 0.5f) : 0f;
            _animator.SetFloat(_animIDSpeed, targetSpeed);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
        }

        if (isMoving)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTarget.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // Gravity
        verticalVelocity += gravity * Time.deltaTime;
//controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
    
    


    void HandleJump()
    {
        
        
        if (!IsOwner) 
        {
            return;
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump, true);
            }
        }
        else if (!isGrounded && verticalVelocity < 0f)
        {
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDFreeFall, true);
            }
        }

        // Zıplama veya düşme bittiğinde animasyonu sıfırla
        if (_hasAnimator && isGrounded)
        {
            _animator.SetBool(_animIDJump, false);
            _animator.SetBool(_animIDFreeFall, false);
        }
    }
}


