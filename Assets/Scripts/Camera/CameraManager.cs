using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera cam1;
    [SerializeField] private Camera cam2;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            cam1.gameObject.SetActive(true);
            cam2.gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            cam2.gameObject.SetActive(true);
            cam1.gameObject.SetActive(false);
        }
    }
}
