using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponObject : MonoBehaviour
{
    public WeaponType weaponType;
    public WeaponEquipType equipType;
    public Vector3 weaponEquipPos;
    public Vector3 weaponEquipRot;
    public Vector3 weaponEquipScale;

    public bool isUse = false;

    public UnityEvent GetWeaponEvent;
}
