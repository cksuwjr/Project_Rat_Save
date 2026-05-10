using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

[Serializable]
public class ShopItemData
{
    public string itemText;
    public Sprite itemSprite;
    public ItemType itemType;
    public float value;
    public int cost;
}

public enum ItemType
{
    AttackPowerPlus,
    AttackSpeedPlus,
    MoveSpeedPlus,
    Heal,
    Else,
}


[Serializable] [CreateAssetMenu(fileName = "StageData", menuName = "ScriptableObject/StageData")]
public class StageData : ScriptableObject
{
    [Tooltip("스테이지마다 각각 설정해주기!")]

    [Header("몬스터 스폰 데이터")]
    public List<SpawnData> spawnDatas;
    [Space(10)]
    [Header("대화 데이터")]
    public List<ChatData> chatDatas;

    [Space(10)]
    [Header("상점 대화")]
    public ChatData shopChatData;

    [Space(10)]
    [Header("상점 아이템 데이터")]
    public List<ShopItemData> shopItemDatas;
}



