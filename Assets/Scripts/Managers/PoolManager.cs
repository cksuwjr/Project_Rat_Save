using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonDestroy<PoolManager>, IManager
{
    public Pool damagePool;

    public Pool monsterPool;

    public Pool punchEffectPool;

    public Pool aoeEffectPool;

    public void Init()
    {
        transform.GetChild(0).TryGetComponent<Pool>(out damagePool);
        transform.GetChild(1).TryGetComponent<Pool>(out monsterPool);

        transform.GetChild(2).TryGetComponent<Pool>(out punchEffectPool);
        transform.GetChild(3).TryGetComponent<Pool>(out aoeEffectPool);


        damagePool?.Init();

        monsterPool?.Init();

        punchEffectPool?.Init();

        aoeEffectPool?.Init();
    }

    public Pool GetPool(int n)
    {
        Pool pool;
        transform.GetChild(n).TryGetComponent<Pool>(out pool);
        return pool;
    }
}