using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (player.keyCollected)
                {
                    player.keyCollected = false;
                    Debug.Log("Door opend!");
                }
                else
                {
                    Debug.Log("Key is required");
                }
            }
        }
    }
}
