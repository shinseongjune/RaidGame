using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    public Transform target;
    public const float SMOOTHNESS = 0.85f;

    void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, SMOOTHNESS);
        }
    }
}
