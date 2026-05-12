using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInput playerInput;
    private Rigidbody2D rb;

    // inputs
    private bool jumpPressed;
    private bool jumpReleased;

    [Header("Movement variables")]
    public float speed = 5f;
    public float jumpForce = 10f;
    public Vector2 moveInput;
    private int facingDirection = 1; // 1 for right, -1 for left

    // following makes gravity smoother for jump and fall
    public float normalGravity;
    public float fallGravity;
    public float jumpGravity;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private bool isGrounded;
    public float jumpCutMultiplier = 0.5f; // cut jump early or max height when jump button is released
    

    private void Start()
    {
        rb.gravityScale = normalGravity;
    }

    /*
     important to use awake instead of start for this, 
    otherwise the player will not move until the second time you hit play 
     */
    void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        flip();
        jumpPressed = playerInput.actions["Jump"].IsPressed();
    }
    void FixedUpdate()
    {
        ApplyVariableGravity();
        checkGrounded();
        handleMovement();
        handleJump();
    }

    private void handleMovement()
    {
        float moveSpeed = moveInput.x * speed;
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
    }

    private void handleJump()
    {
        if (isGrounded && jumpPressed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpPressed = false;
            jumpReleased = false;
        }
        if (jumpReleased)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }
        jumpReleased = false;
    }

    void ApplyVariableGravity()
    {
        /* 0.1 makes sure that the slightest upward or downward movement is not considered as going up or down, 
        which would cause the gravity to switch back and forth rapidly (appears to stutter)
        */
        if (rb.linearVelocity.y > 0.1f) // going up
        {
            rb.gravityScale = jumpGravity;
        }
        else if (rb.linearVelocity.y < 0.1f) // going down
        {
            rb.gravityScale = fallGravity;
        }
        else
        {
            rb.gravityScale = normalGravity;
        }
    }

    void checkGrounded()
    {
        // parameters: position of the circle, radius of the circle, layer to check against
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer); 
    }
    void flip()
    {
        if (moveInput.x > 0.1f)
        {
            facingDirection = 1;

        }
        else if (moveInput.x < -0.1f){
            facingDirection = -1;
        }
        transform.localScale = new Vector3(facingDirection, 1, 1);
    }
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    public void OnJump(InputValue value)
    {
       if (value.isPressed)
       {
            jumpPressed = true;
            jumpReleased = false;
       }
       else
       {
            jumpPressed = false;
            jumpReleased = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

}
