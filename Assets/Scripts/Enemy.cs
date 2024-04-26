using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float chaseRange = 5f;
    public Transform groundDetection;

    private bool isFacingRight = true;
    private bool isChasing = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
            Patrol();
        }

        if (isChasing)
        {
            ChasePlayer();
        }
    }

    bool CanSeePlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, chaseRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }

    void Patrol()
    {
        Vector2 movement = Vector2.right * moveSpeed * Time.deltaTime;

        if (!isFacingRight)
        {
            movement *= -1f;
        }

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 0.5f);
        if (!groundInfo.collider)
        {
            Flip();
        }

        rb.MovePosition(rb.position + movement);
    }

    void ChasePlayer()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 movement = direction * moveSpeed * Time.deltaTime;

        if (movement.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (movement.x < 0 && isFacingRight)
        {
            Flip();
        }

        rb.MovePosition(rb.position + movement);
    }
    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
