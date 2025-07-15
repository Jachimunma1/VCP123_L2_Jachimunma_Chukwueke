using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null) Debug.Log($"RigidBody Exists {rb.name}");
        else Debug.LogError("RigidBody2D component not found on PlayerController GameObject.");
    }

    // Update is called once per frame
    void Update()
    {
        float hValue = Input.GetAxis("Horizontal");
        //GetAxisRaw returns the value of the axis with no smoothing

        rb.linearVelocityX = hValue * 5f; // Adjust speed as necessary

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse); // Adjust jump force as necessary
        }
    }
}