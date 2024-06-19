using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.CollectKey();
                NotifyDoor();
                Destroy(gameObject);
            }
        }
    }

    public void NotifyDoor()
    {
        Door door = UnityEngine.Object.FindFirstObjectByType<Door>();
        if (door != null)
        {
            door.KeyCollected();
        }
    }
}