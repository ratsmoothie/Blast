using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 cameraOffset;
    
    //LateUpdate is apparently common for camera scripts
    void LateUpdate()
    {
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + cameraOffset;
        }
    }
}
