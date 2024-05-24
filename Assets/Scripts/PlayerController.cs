using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 10f;
    public float runSpeed = 30f;
    public float jumpForce = 10f;
    public float iceControlFactor = 0.3f;

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool onIce = false;
    public bool keyCollected = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0.5f;
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");

        float currentSpeed = onIce ? walkSpeed * iceControlFactor : (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed);
        rb.velocity = new Vector2(moveHorizontal * currentSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.GetComponent<Ice>() != null)
        {
            isGrounded = true;
            onIce = collision.gameObject.GetComponent<Ice>() != null;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.GetComponent<Ice>() != null)
        {
            isGrounded = true;
            onIce = collision.gameObject.GetComponent<Ice>() != null;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.GetComponent<Ice>() != null)
        {
            isGrounded = false;
            if (collision.gameObject.GetComponent<Ice>() != null)
            {
                onIce = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lava"))
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player has died");
    }
}