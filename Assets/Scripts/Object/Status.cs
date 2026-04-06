using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] private float maxHp;
    [SerializeField] private float hp;
    [SerializeField] private float attackPower;
    [SerializeField] private float moveSpeed;

    public float MaxHP
    {
        get { return maxHp; }
        set { maxHp = value; }
    }

    public float HP
    {
        get { return hp; }
        set { hp = value; }
    }

    public float AttackPower
    {
        get { return attackPower; }
        set { attackPower = value; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
}