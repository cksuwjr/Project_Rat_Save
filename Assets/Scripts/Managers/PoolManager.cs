using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonDestroy<PoolManager>, IManager
{
    public Pool damagePool;

    public Pool monsterPool;

    public void Init()
    {
        transform.GetChild(0).TryGetComponent<Pool>(out damagePool);
        transform.GetChild(1).TryGetComponent<Pool>(out monsterPool);


        damagePool?.Init();

        monsterPool?.Init();
    }
}