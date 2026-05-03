using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraMode
{
    CAM1,
    CAM2,
    CAM3,
}

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera cam1;
    [SerializeField] private Camera cam2;
    [SerializeField] private Camera cam3;

    private Vector3 offset;
    float fixedPitch;

    public static CameraMode cameraMode;

    // Update is called once per frame

    private void Awake()
    {
        if (cam1.gameObject.activeSelf) CameraManager.cameraMode = CameraMode.CAM1;
        if (cam2.gameObject.activeSelf) CameraManager.cameraMode = CameraMode.CAM2;
        if (cam3.gameObject.activeSelf) CameraManager.cameraMode = CameraMode.CAM3;

        offset = cam3.transform.position - GameManager.Instance.Player.transform.position;
        fixedPitch = cam3.transform.eulerAngles.x;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            cam1?.gameObject.SetActive(true);
            cam2?.gameObject.SetActive(false);
            cam3?.gameObject.SetActive(false);

            CameraManager.cameraMode = CameraMode.CAM1;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            cam1?.gameObject.SetActive(false);
            cam2?.gameObject.SetActive(true);
            cam3?.gameObject.SetActive(false);

            CameraManager.cameraMode = CameraMode.CAM2;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            cam1?.gameObject.SetActive(false);
            cam2?.gameObject.SetActive(false);
            cam3?.gameObject.SetActive(true);

            CameraManager.cameraMode = CameraMode.CAM3;
        }

        if(CameraManager.cameraMode == CameraMode.CAM3)
        {
            if (Input.GetMouseButton(0)) // 드래그할 때만
            {
                if (Input.GetMouseButton(0))
                {
                    float mouseX = Input.GetAxis("Mouse X");
                    float rotY = mouseX * 300 * Time.deltaTime;

                    // offset을 Y축 기준으로 회전
                    offset = Quaternion.AngleAxis(rotY, Vector3.up) * offset;
                }
            }
        }

        if (Input.GetMouseButtonDown(0)) // 좌클릭
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.CompareTag("ShopOwner"))
                {
                    Debug.Log("e");
                    UIManager.Instance.UI_Chat_OpenClose();
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (CameraManager.cameraMode != CameraMode.CAM3) return;

        // 플레이어 기준으로 카메라 위치 유지
        cam3.transform.position = GameManager.Instance.Player.transform.position + offset;

        Vector3 dir = GameManager.Instance.Player.transform.position - cam3.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.001f)
        {
            float targetY = Quaternion.LookRotation(dir).eulerAngles.y;

            cam3.transform.rotation = Quaternion.Euler(fixedPitch, targetY, 0f);
        }
    }
}
