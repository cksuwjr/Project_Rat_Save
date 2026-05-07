using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour, IMove
{
    private Animator animator;
    private Rigidbody rb;

    // 이동량 계산을 위한 벡터
    private Vector3 moveDelta;

    public Vector3 Direction { get; set; }

    public bool Movable { get; set; }


    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        TryGetComponent<Rigidbody>(out rb);
        Movable = true;
    }

    public void Move(Vector3 direction, float speed)
    {
        if (!Movable) return;
        animator?.SetBool("Move", false);
        if (direction == Vector3.zero) return;

        moveDelta.x = direction.x;
        moveDelta.y = 0;
        moveDelta.z = direction.z;

        animator?.SetBool("Move", direction.sqrMagnitude > 0.01f);

        if (direction != Vector3.zero)
        {
            moveDelta.Normalize();
            moveDelta *= speed * Time.deltaTime;

            
            rb.MovePosition(rb.position + moveDelta);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion newRotation = Quaternion.RotateTowards(
                rb.rotation,
                targetRotation,
                1500f * Time.deltaTime
            );

            rb.MoveRotation(newRotation);

            Direction = direction;
        }
    }

    public void Rolling(float speed)
    {
        moveDelta.x = Direction.x;
        moveDelta.y = 0;
        moveDelta.z = Direction.z;

        moveDelta.Normalize();
        moveDelta *= speed * Time.deltaTime;

        rb.MovePosition(rb.position + moveDelta);
    }

    public void See(Entity entity)
    {
        var lookRot = entity.transform.position - transform.position;
        lookRot.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(lookRot);
        Quaternion newRotation = Quaternion.RotateTowards(
            rb.rotation,
            targetRotation,
            1500f * Time.deltaTime
        );

        rb.MoveRotation(newRotation);

        Direction = lookRot.normalized;
    }
}