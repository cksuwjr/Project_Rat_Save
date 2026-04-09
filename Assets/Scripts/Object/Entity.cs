using System;
using UnityEngine;

public class Entity : PoolObject
{
    protected Status status;

    public event Action OnDie;
    public Action<float, float, float> OnChangeHp;

    public LayerMask enemyLayer;

    public bool isDead = false;

    private void Awake()
    {
        TryGetComponent<Status>(out status);
        DoAwake();
    }

    protected virtual void DoAwake()
    {
        isDead = false;
    }

    public virtual void GetDamage(Entity attacker, float damage, float knockbackTime = 3f)
    {
        if (isDead) return;

        status.HP -= damage;
        
        var damageText = PoolManager.Instance.damagePool.GetPoolObject();
        if (damageText.TryGetComponent<DamageObject>(out var damageObj))
        {
            damageObj.transform.position = transform.position + Camera.main.transform.up * 2;
            damageObj.Init(damage);
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
        ReturnToPool();
    }
}