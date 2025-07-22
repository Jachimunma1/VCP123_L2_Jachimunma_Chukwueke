using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float groundCheckRadius = 0.2f; // Radius for ground detection, adjust as necessary

    [SerializeField] private bool isGrounded = false; // Flag to check if the player is on the ground
    private LayerMask groundLayer; // Layer for ground detection

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D col;
    private Animator anim;

    private int maxJumpCount = 2; // Maximum number of jumps allowed (for double jump or similar mechanics)
    private int jumpCount = 1; // Counter for jumps, if you want to implement double jump or similar mechanics  
    private bool isJumping = false; // Flag to check if the player is jumping

    private Vector2 groundCheckPos => new Vector2(col.bounds.min.x + col.bounds.extents.x, col.bounds.min.y);
    //Vector2 GetGroundCheckPos(); for calculating the ground check position but we did it in one line above
    //{
    //Calculate the ground check position based on the player's position and collider size
    //return new Vector2(col.bounds.min.x + col.bounds.extents.x, col.bounds.min.y);
    // }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();

        groundLayer = LayerMask.GetMask("Ground"); // Assuming you have a layer named "Ground"

        if (groundLayer == 0)
            Debug.LogError("Ground layer not found. Please ensure you have a layer named 'Ground'.");

        sr.flipX = false; // Ensure the sprite is not flipped by default

        if (rb != null) Debug.Log($"RigidBody Exists {rb.name}");
        else Debug.LogError("RigidBody2D component not found on PlayerController GameObject.");
    }

    // Update is called once per frame
    void Update()
    {
        float hValue = Input.GetAxis("Horizontal");
        SpriteFlip(hValue);
        // Fix for CS0029: Cannot implicitly convert type 'bool' to 'float'
        bool fireButtonPressed = Input.GetButtonDown("Fire1");
        float aValue = fireButtonPressed ? 1f : 0f;
        //groundCheckPos = GetGroundCheckPos(); for the ground check position but we did it in one line above
        //GetAxisRaw returns the value of the axis with no smoothing

        rb.linearVelocityX = hValue * 5f; // Adjust speed as necessary
        isGrounded = Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            rb.linearVelocityY = 0f; // Reset vertical velocity before jumping
            rb.AddForce(Vector2.up * 7f, ForceMode2D.Impulse); // Adjust jump force as necessary
            jumpCount++;
        }

        if (isGrounded)
        {
            jumpCount = 1; // Reset jump count when grounded
        }

        if (Input.GetButtonDown("Fire1"))
        {
            aValue = 1f;
            // Handle fire action here, e.g., shooting or attacking
            Debug.Log("Fire action triggered");
        }

        if (!isGrounded && rb.linearVelocity.y > 0)
        {
            isJumping = true; // Player is jumping
        }
        else if (!isGrounded && rb.linearVelocity.y <= 0)
        {
            isJumping = false; // Player is not jumping
        }

        anim.SetFloat("hValue", Mathf.Abs(hValue));
        anim.SetFloat("aValue", Mathf.Abs(aValue));
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isJumping", false);
    }

    void SpriteFlip(float hValue) 
    {
        // Flip the sprite based on horizontal input all the different ways to do it
        // if (hValue < 0)
        //     sr.flipX = true; // Flip the sprite to face left
        // else if (hValue > 0)
        //     sr.flipX = false; // Flip the sprite to face right

        if (hValue !=0) sr.flipX = (hValue < 0); // Flip the sprite based on horizontal input 

       // if (hValue < 0 && sr.flipX) || (hValue > 0 && !sr.flipX)) {
       //     sr.flipX = !sr.flipX; // Toggle the flip state
       //  }
    }

    public void CheckIsGrounded()
    {
        if (!isGrounded && rb.linearVelocityY < 0)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, groundLayer);
            // If the player is not grounded and falling, reset jump count
            jumpCount = 1;
        }
        else if (!isGrounded && rb.linearVelocityY > 0)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, groundLayer);
            // If the player is not grounded and jumping, allow for another jump   
            jumpCount = 1;
        }
        else if (isGrounded)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, groundLayer);
        }
    }
}