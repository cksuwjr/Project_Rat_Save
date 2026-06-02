using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonDestroy<PoolManager>, IManager
{
    public Pool damagePool;

    public Pool punchEffectPool;

    public Pool aoeEffectPool;

    public Pool yellowCatPool;

    public Pool blackCatPool;
    
    public Pool arrowPool;

    public Pool leeshinQPool;

    public Pool jewelPool;

    public Pool bulletPool;

    public void Init()
    {
        int count = 0;

        transform.GetChild(count++).TryGetComponent<Pool>(out damagePool);

        transform.GetChild(count++).TryGetComponent<Pool>(out punchEffectPool);
        transform.GetChild(count++).TryGetComponent<Pool>(out aoeEffectPool);

        transform.GetChild(count++).TryGetComponent<Pool>(out yellowCatPool);
        transform.GetChild(count++).TryGetComponent<Pool>(out blackCatPool);

        transform.GetChild(count++).TryGetComponent<Pool>(out arrowPool);

        transform.GetChild(count++).TryGetComponent<Pool>(out leeshinQPool);

        transform.GetChild(count++).TryGetComponent<Pool>(out jewelPool);

        transform.GetChild(count++).TryGetComponent<Pool>(out bulletPool);

        damagePool?.Init();

        punchEffectPool?.Init();

        aoeEffectPool?.Init();

        yellowCatPool?.Init();

        blackCatPool?.Init();

        arrowPool?.Init();

        leeshinQPool?.Init();

        jewelPool?.Init();

        bulletPool?.Init();
    }

    public Pool GetPool(int n)
    {
        Pool pool;
        transform.GetChild(n).TryGetComponent<Pool>(out pool);
        return pool;
    }
}