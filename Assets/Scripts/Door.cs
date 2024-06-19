using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animation hingeAnimation;
    private bool isOpen = false;

    public void KeyCollected()
    {
        if (!isOpen)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        hingeAnimation.Play();
        isOpen = true;
    }
}
