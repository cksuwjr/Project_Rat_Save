using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour, IMove
{
    //[SerializeField] private LayerMask wallLayer;
    [SerializeField] private float moveSpeed = 5f; // 기본값 5

    private Animator animator;
    private Rigidbody rb;

    // 이동량 계산을 위한 벡터
    private Vector3 moveDelta;

    private Vector3 finalDir;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        TryGetComponent<Rigidbody>(out rb);
    }

    public void SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public void Move(Vector3 direction)
    {
        moveDelta.x = direction.x;
        moveDelta.y = 0;
        moveDelta.z = direction.z;

        animator?.SetBool("Move", direction.sqrMagnitude > 0.01f);

        if (direction != Vector3.zero)
        {
            moveDelta.Normalize();
            moveDelta *= moveSpeed * Time.deltaTime;

            
            rb.MovePosition(rb.position + moveDelta);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion newRotation = Quaternion.RotateTowards(
                rb.rotation,
                targetRotation,
                1500f * Time.deltaTime
            );

            rb.MoveRotation(newRotation);

            finalDir = direction;
        }
    }

    public void Rolling(float speed)
    {
        moveDelta.x = finalDir.x;
        moveDelta.y = 0;
        moveDelta.z = finalDir.z;

        moveDelta.Normalize();
        moveDelta *= speed * Time.deltaTime;

        rb.MovePosition(rb.position + moveDelta);
    }
}