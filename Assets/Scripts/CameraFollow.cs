using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 2f, -5f);
    public float zoomFactor = 1.0f;
    public float zoomSpeed = 2.0f;
    public float minZoom = 1.0f;
    public float maxZoom = 5.0f;

    private void LateUpdate()
    {
        if (target != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            zoomFactor -= scroll * zoomSpeed;
            zoomFactor = Mathf.Clamp(zoomFactor, minZoom, maxZoom);

            Vector3 zoomedOffset = offset * zoomFactor;

            transform.position = target.position + zoomedOffset;
            transform.LookAt(target);
        }
    }
}
