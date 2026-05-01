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

    public void Init()
    {
        int count = 0;

        transform.GetChild(count++).TryGetComponent<Pool>(out damagePool);

        transform.GetChild(count++).TryGetComponent<Pool>(out punchEffectPool);
        transform.GetChild(count++).TryGetComponent<Pool>(out aoeEffectPool);

        transform.GetChild(count++).TryGetComponent<Pool>(out yellowCatPool);
        transform.GetChild(count++).TryGetComponent<Pool>(out blackCatPool);



        damagePool?.Init();

        punchEffectPool?.Init();

        aoeEffectPool?.Init();

        yellowCatPool?.Init();

        blackCatPool?.Init();
    }

    public Pool GetPool(int n)
    {
        Pool pool;
        transform.GetChild(n).TryGetComponent<Pool>(out pool);
        return pool;
    }
}