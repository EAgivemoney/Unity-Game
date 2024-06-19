using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{
    public float speed = 1.5f;
    public int health = 1; // Reduced health to 1

    private Transform playerTransform;
    private float targetYPosition = 49.06106f;
    private float fixedZPosition = -2.42f;

    public event Action OnDeath;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found. Ensure player is tagged with 'Player'.");
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            Vector3 targetPosition = new Vector3(playerTransform.position.x, targetYPosition, fixedZPosition);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            Debug.LogWarning("Player transform is not assigned.");
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        OnDeath?.Invoke(); // Ensure to invoke event if there are subscribers
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Fireball fireball = collision.gameObject.GetComponent<Fireball>();
        if (fireball != null)
        {
            TakeDamage(fireball.damage);
            Destroy(fireball.gameObject); // Destroy the fireball on collision
        }
        else if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage();
            }
        }
    }
}