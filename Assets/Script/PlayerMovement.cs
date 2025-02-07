using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 9.0f;
    public float turnSpeed = 200.0f;
    public float jumpForce = 1000.0f;
    public float groundCheckDistance = 0.5f;

    private float horizontalInput;
    private float forwardInput;
    private Rigidbody rb;
    private Animator animator;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        GetInput();
        UpdateAnimations();
        HandleJump();
    }

    void FixedUpdate()
    {
        Move();
        Rotate();
        CheckGrounded();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        forwardInput = Input.GetAxis("Vertical");
    }

    private void UpdateAnimations()
    {
        float movementSpeed = Mathf.Clamp01(Mathf.Abs(forwardInput) + Mathf.Abs(horizontalInput));
        animator.SetFloat("Speed_f", movementSpeed);
        animator.SetBool("Grounded", isGrounded);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            animator.SetBool("Jump_b", true);
        }
    }

    private void Move()
    {
        Vector3 movement = transform.forward * forwardInput * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private void Rotate()
    {
        float turn = horizontalInput * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, turn, 0);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    private void CheckGrounded()
    {
        RaycastHit hit;
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance);
        animator.SetBool("Grounded", isGrounded);

        if (isGrounded)
        {
            animator.SetBool("Jump_b", false);
        }
    }
}
