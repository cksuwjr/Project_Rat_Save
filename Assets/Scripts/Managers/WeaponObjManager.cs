using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObjManager : SingletonDestroy<WeaponObjManager>, IManager
{
    public WeaponObject woodCarving;
    public WeaponObject glove;
    public WeaponObject sword;

    public WeaponObject bow;
    public WeaponObject gun;
    public WeaponObject shotGun;

    public WeaponObject fireExtinguisher;   // 소화기
    public WeaponObject leeShin;            // 리신
    public WeaponObject picaChu;            // 피카츄




    public void Init()
    {
        //int count = 0;

        //transform.GetChild(count++).TryGetComponent<Pool>(out damagePool);

        //transform.GetChild(count++).TryGetComponent<Pool>(out punchEffectPool);
        //transform.GetChild(count++).TryGetComponent<Pool>(out aoeEffectPool);

        //transform.GetChild(count++).TryGetComponent<Pool>(out yellowCatPool);
        //transform.GetChild(count++).TryGetComponent<Pool>(out blackCatPool);

    }
}
