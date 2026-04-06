using System;
public class Entity : PoolObject
{
    protected Status status;

    public event Action OnDie;

    private void Awake()
    {
        TryGetComponent<Status>(out status);
        DoAwake();
    }

    protected virtual void DoAwake()
    {

    }

    public virtual void GetDamage(Entity attacker, float damage, float knockbackTime = 3f)
    {
        status.HP -= damage;
        if (status.HP <= 0)
            Die();
    }

    protected virtual void Die()
    {
        OnDie?.Invoke();
        ReturnToPool();
    }
}