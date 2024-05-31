using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    private void Reset()
    {
        if (!GetComponent<IcePhysics>())
        {
            gameObject.AddComponent<IcePhysics>();
        }
    }
}