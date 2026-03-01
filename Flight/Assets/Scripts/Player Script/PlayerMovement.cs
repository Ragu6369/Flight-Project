using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Values")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float lowSpeed = 0.5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float groundCheckDistance = 1.1f;
    [SerializeField] private float GravityTimer = 0f;
    [SerializeField] private float runSpeedLimit = 12f;
    [SerializeField] private float acceleration = 3f;


    public Transform cameraArm;
    public Animator anim;

    public LayerMask groundLayer;

    private Rigidbody rb;
    private Vector3 moveInput;
    private float horizontalInput;
    private float verticalInput;
    private Vector3 camForward;
    private Vector3 camRight;
    [SerializeField] private bool IsJumping = false;
    [SerializeField] private bool forceForawrd = false;
    [SerializeField] private bool IsMoving = false;
    [SerializeField] private bool Grounded = false;
    public float currentSpeed;
    public float fallCheck = 8f;
    public float rollcheck = 3f;
    public float aboveGroundCheck = 8f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentSpeed = lowSpeed;
    }

    private void Update()
    {
        Grounded = IsGrounded();
        // Input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Camera-relative movement
        camForward = cameraArm.forward; camForward.y = 0; camForward.Normalize();
        camRight = cameraArm.right; camRight.y = 0; camRight.Normalize();

        moveInput = (camForward * verticalInput + camRight * horizontalInput).normalized;

        // override input if isforce forward is true
        if (forceForawrd)
        {
            moveInput = camForward; // straight in cam forward 

        }


        MoveLogic();
        JumpLogic();

        // Animator states
        anim.SetBool("IsWalking",IsMoving);
        anim.SetBool("IsRunning", moveInput != Vector3.zero && Input.GetButton("Run"));
        anim.SetBool("IsJumping", IsJumping);

        // fall check height
        HighCheck(fallCheck);

        // roll check height
        RollingCheck(rollcheck);

        if(IsMoving && !Input.GetButton("Run"))
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, acceleration * Time.deltaTime);
        }
        if(!IsMoving)
        {
            currentSpeed = moveSpeed;
        }
        
        // Speed control for running
        if (Input.GetButton("Run") && moveInput != Vector3.zero)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, runSpeedLimit, acceleration * Time.deltaTime);
            anim.SetBool("IsWalking", true);
            anim.SetBool("IsRunning", true);


        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed, acceleration * Time.deltaTime);
        }

        
    }

    private void FixedUpdate()
    {
        MovePlayer();
        OnJump();
    }

    private void MoveLogic()
    {

        if (moveInput != Vector3.zero)
        {
            IsMoving = true;
        }
        else
        {
            IsMoving = false;
        }
    }

    private void JumpLogic()
    {


        if (!IsGrounded())
        {
            AboveGrounds();
        }

        if (IsGrounded() && IsJumping == true)
        {
            GravityTimer += Time.deltaTime;
            if (GravityTimer >= 1f) // time after which player can jump again
            {
                IsJumping = false;
                GravityTimer = 0f;
            }
        }

    }

    private void MovePlayer()
    {
        rb.MovePosition(rb.position + moveInput * currentSpeed * Time.fixedDeltaTime);

        
        if (moveInput != Vector3.zero )
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveInput);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
        
    }

    private void OnJump()
    {
        if (Input.GetButton("Jump") && IsGrounded() && !IsJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            IsJumping = true;
            anim.SetBool("IsJumping", true);
            anim.SetBool("IsRolling", false);
        }
    
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }

    public void ForceForwardForSeconds(float duration)
    {
        StartCoroutine(ForceForwardRoutine(duration));
    }



    private IEnumerator ForceForwardRoutine(float duration)
    {
        forceForawrd = true;
        yield return new WaitForSeconds(duration);
        forceForawrd = false;
    }

    private void HighCheck(float rayLength)
    {
        bool hit = Physics.Raycast(transform.position, Vector3.down, rayLength);
        if (IsJumping && !hit && !IsGrounded())
        {
            anim.SetBool("IsFalling ", true);
        }
        else
        {
            anim.SetBool("IsFalling ", false);
        }
    }

    private void AboveGrounds()
    {
       bool hit = Physics.Raycast(transform.position, Vector3.down,aboveGroundCheck,groundLayer);

       if(!hit)
       {
            if(!IsJumping)
            {
                IsJumping = true;
                anim.SetBool("IsJumping", true);
            }
            
       }

    }
    private void RollingCheck(float rayLength)
    {
        if (IsJumping && !IsGrounded())
        {
            bool hit = Physics.Raycast(transform.position, Vector3.down, rayLength);

            if (hit)
            {
                anim.SetBool("IsRolling", true);
            }
            else
            {
                anim.SetBool("IsRolling", false);
            }
        }
        else
        {
            anim.SetBool("IsRolling", false);
        }
        
        

    }
}
