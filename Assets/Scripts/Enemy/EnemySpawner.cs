using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private int id;

    public static EnemySpawner GetSpawnerById(int id)
    {
        EnemySpawner[] spawners = GameObject.FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);

        foreach(EnemySpawner spawner in spawners)
            if(spawner.id == id)
                return spawner;

        return null;
    }

    public GameObject Spawn(int spawnId, WeaponType weapon = WeaponType.Hand)
    {
        var spawned = PoolManager.Instance.GetPool(spawnId).GetPoolObject();
        var spawnPosition = transform.position;
        spawnPosition.y = 0.5f;
        spawned.transform.position = spawnPosition;

        var hand = spawned.GetComponent<WeaponManager>().hand;
        WeaponObject spawnedWeapon;
        switch(weapon)
        {
            case WeaponType.Hand:
                break;
            case WeaponType.Wood_Carving:
                spawnedWeapon = Instantiate(WeaponObjManager.Instance.woodCarving, hand);

                spawnedWeapon.GetComponent<BoxCollider>().isTrigger = true;

                Destroy(spawnedWeapon.GetComponent<Rigidbody>());
                //weapon.GetComponent<Rigidbody>().useGravity = false;

                spawnedWeapon.transform.localPosition = spawnedWeapon.weaponEquipPos;
                spawnedWeapon.transform.localRotation = Quaternion.Euler(spawnedWeapon.weaponEquipRot);
                spawnedWeapon.gameObject.transform.localScale = spawnedWeapon.weaponEquipScale;

                spawnedWeapon.isUse = true;


                break;
        }

        spawned.GetComponent<WeaponManager>().ChangeWeapon(weapon);

        return spawned;
    }
}
