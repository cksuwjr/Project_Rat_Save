using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRotater : MonoBehaviour
{
    void Update()
    {
        Vector3 parentEuler = transform.parent.rotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(Camera.main.transform.rotation.eulerAngles.x, -parentEuler.y, 0);
    }
}
