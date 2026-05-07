using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Entity : PoolObject
{
    protected Status status;

    public event Action OnDie;
    public Action<float, float, float> OnChangeHp;
    public Action OnInit;

    public LayerMask enemyLayer;

    public bool isDead = false;

    public virtual Vector3 GetDirection { get; }

    private void Awake()
    {
        TryGetComponent<Status>(out status);
        DoAwake();
    }

    protected virtual void DoAwake()
    {
        isDead = false;
    }

    public virtual void Init(float hp, float speed) 
    { 
        status.MaxHP = status.HP = hp;

        status.MoveSpeed = speed;

        isDead = false;

        OnInit?.Invoke();
    }

    public virtual void GetDamage(Entity attacker, float damage, SkillType skillType, float knockbackTime = 3f, int effectNum = 0)
    {
        if (isDead) return;

        var finalDamage = UnityEngine.Random.Range(damage * 0.85f, damage * 1.15f);

        //status.HP -= damage;
        status.HP -= finalDamage;

        var damageText = PoolManager.Instance.damagePool.GetPoolObject();
        if (damageText.TryGetComponent<DamageObject>(out var damageObj))
        {
            var floatPos = transform.position;
            floatPos.y = 0;
            floatPos += (Camera.main.transform.up - Camera.main.transform.forward).normalized * 4;
            damageObj.transform.position = floatPos;

            //damageObj.Init(damage);
            damageObj.Init(finalDamage);

            if (effectNum != 0)
            {
                var effect = PoolManager.Instance.GetPool(effectNum).GetPoolObject();
                effect.transform.position = transform.position;
                effect.GetComponent<Effect>().Init();
            }
        }
        if (status.HP <= 0)
        {
            status.HP = 0;
            Die();
        }

    }

    protected virtual void Die()
    {
        isDead = true;

        OnDie?.Invoke();
        Invoke("ReturnToPool", 1.5f);
    }

    public virtual void StopAct() { }
    public virtual void StartAct() { }

    public virtual List<Entity> GetNearEnemys(float range)
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, range);

        List<Entity> enemys = new List<Entity>();

        foreach (Collider col in cols)
        {
            if (col.isTrigger) continue;

            if ((enemyLayer & (1 << col.gameObject.layer)) != 0)
            {
                var enemy = col.GetComponent<Entity>();

                if (enemy)
                    if (enemy.isDead) continue;
                    else enemys.Add(enemy);

            }
        }

        // 가장 가까운 순서 정렬
        enemys.Sort((a, b) =>
            Vector3.Distance(a.transform.position, transform.position)
            .CompareTo(
            Vector3.Distance(b.transform.position, transform.position)
            ));
        
        return enemys;
    }

    public Entity GetNearEnemy(float range)
    {
        var enemys = GetNearEnemys(range);
        
        return (enemys.Count > 0) ? enemys[0] : null;
    }
}