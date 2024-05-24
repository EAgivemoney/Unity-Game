using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    private PhysicsMaterial2D iceMaterial;

    private void Start()
    {
        iceMaterial = new PhysicsMaterial2D();
        iceMaterial.friction = 0.1f;
        iceMaterial.bounciness = 0f;
    }

    void OnCollisonEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.sharedMaterial = iceMaterial;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.sharedMaterial = null;
        }
    }
}
