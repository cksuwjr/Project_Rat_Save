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

[Serializable]
public class ChatData
{
    public string nameText;
    public string chatText;
    public Sprite saySprite;

    public string selectBtn1_Text;
    public string selectBtn2_Text;

}


[Serializable] [CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObject/StageData")]
public class StageData : ScriptableObject
{
    public List<SpawnData> spawnDatas;
    public List<ChatData> chatDatas;

    public ChatData shopChatData;
}



