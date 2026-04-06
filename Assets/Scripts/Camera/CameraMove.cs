using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform objectTofollow;
    public float followSpeed = 10f;

    public float smoothness = 10f;

    private Vector3 dist;

    private bool init = false;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        init = true;

        objectTofollow = GameManager.Instance.Player.transform;
        dist = transform.position - objectTofollow.position;
    }


    private void LateUpdate()
    {
        if (!init) return;

        transform.position = Vector3.MoveTowards(transform.position, objectTofollow.position + dist, followSpeed * Time.deltaTime);
    }
}