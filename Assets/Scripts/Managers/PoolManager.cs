using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonDestroy<PoolManager>, IManager
{
    public Pool monsterPool;

    public void Init()
    {
        monsterPool?.Init();
    }
}