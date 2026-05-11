using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PoolObject
{
    private Entity attacker;
    private Vector3 direction;
    private float damage;
    private float speed;

    private SkillType type;

    public Action<GameObject> OnHitEvent;

    public void Init(Entity attacker, Vector3 direction, float damage, float speed, float duration = 3f, SkillType skillType = SkillType.Base)
    {
        this.attacker = attacker;
        this.direction = direction;
        this.direction.y = 0f;
        this.damage = damage;
        this.speed = speed;

        this.type = skillType;

        transform.forward = this.direction;

        Invoke("ReturnToPool", duration);

    }

    private void FixedUpdate()
    {
        transform.position += direction * speed * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == attacker.tag) return;
        if (!other.GetComponent<Entity>()) return;
        if (other.isTrigger) return;

        other.GetComponent<Entity>().GetDamage(attacker, damage, type);
        OnHitEvent?.Invoke(other.gameObject);

        CancelInvoke("ReturnToPool");
        ReturnToPool();
    }
}
