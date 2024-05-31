using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 10f;
    public float runSpeed = 30f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool onIce = false;
    public bool keyCollected = false;

    private float normalFriction = 0.4f;
    private float iceControlFactor = 1f;

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
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.GetComponent<IcePhysics>() != null)
        {
            isGrounded = true;
            if (collision.gameObject.GetComponent<IcePhysics>() != null)
            {
                onIce = true;
                ApplyIcePhysics(collision.gameObject.GetComponent<IcePhysics>());
            }
            else
            {
                onIce = false;
                ApplyNormalPhysics();
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.GetComponent<IcePhysics>() != null)
        {
            isGrounded = true;
            if (collision.gameObject.GetComponent<IcePhysics>() != null)
            {
                onIce = true;
                ApplyIcePhysics(collision.gameObject.GetComponent<IcePhysics>());
            }
            else
            {
                onIce = false;
                ApplyNormalPhysics();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.GetComponent<IcePhysics>() != null)
        {
            isGrounded = false;
            if (collision.gameObject.GetComponent<IcePhysics>() != null)
            {
                onIce = false;
                ApplyNormalPhysics();
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

    void ApplyIcePhysics(IcePhysics ice)
    {
        iceControlFactor = ice.iceControlFactor;
        var material = rb.sharedMaterial;
        material.friction = ice.iceFriction;
        rb.sharedMaterial = material;
    }

    void ApplyNormalPhysics()
    {
        iceControlFactor = 1f;
        var material = rb.sharedMaterial;
        material.friction = normalFriction;
        rb.sharedMaterial = material;
    }

    void Die()
    {
        Debug.Log("Player has died");
    }
}