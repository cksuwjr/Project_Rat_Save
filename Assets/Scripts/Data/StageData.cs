using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[Serializable]
public class SpawnData
{
    public int spawnerID;
    public EnemyType spawnType;
    public WeaponType weaponType;
    public float hp;
    public float speed;
    public float spawnTerm;
    public int spawnCount;
}

[Serializable] [CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObject/StageData")]
public class StageData : ScriptableObject
{
    public List<SpawnData> spawnDatas;
}
