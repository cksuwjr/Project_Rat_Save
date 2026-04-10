using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : PoolObject
{
    public void Init()
    {
        Invoke("ReturnToPool", 1f);
    }
}