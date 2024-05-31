using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animation hingehere;

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
            hingehere.Play();
    }
}
