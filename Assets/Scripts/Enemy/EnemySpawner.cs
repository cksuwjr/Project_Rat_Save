using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

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

    public GameObject Spawn(EnemyType spawnType, WeaponType weapon = WeaponType.Hand)
    {
        var spawnId = 0;
        switch(spawnType)
        {
            case EnemyType.OrangeCat:
                spawnId = 3;
                break;
            case EnemyType.BlackCat:
                spawnId = 4;
                break;
        }

        var spawned = PoolManager.Instance.GetPool(spawnId).GetPoolObject();
        var spawnPosition = transform.position;
        spawnPosition.y = 0f;
        spawned.transform.position = spawnPosition;


        var hand = spawned.GetComponent<WeaponManager>().hand;
        var head = spawned.GetComponent<WeaponManager>().head;
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

            case WeaponType.Glove:
                spawnedWeapon = Instantiate(WeaponObjManager.Instance.glove, hand);

                spawnedWeapon.GetComponent<BoxCollider>().isTrigger = true;

                Destroy(spawnedWeapon.GetComponent<Rigidbody>());
                //weapon.GetComponent<Rigidbody>().useGravity = false;

                spawnedWeapon.transform.localPosition = spawnedWeapon.weaponEquipPos;
                spawnedWeapon.transform.localRotation = Quaternion.Euler(spawnedWeapon.weaponEquipRot);
                spawnedWeapon.gameObject.transform.localScale = spawnedWeapon.weaponEquipScale;

                spawnedWeapon.isUse = true;
                break;
            case WeaponType.Sword:
                spawnedWeapon = Instantiate(WeaponObjManager.Instance.sword, hand);

                spawnedWeapon.GetComponent<BoxCollider>().isTrigger = true;

                Destroy(spawnedWeapon.GetComponent<Rigidbody>());
                //weapon.GetComponent<Rigidbody>().useGravity = false;

                spawnedWeapon.transform.localPosition = spawnedWeapon.weaponEquipPos;
                spawnedWeapon.transform.localRotation = Quaternion.Euler(spawnedWeapon.weaponEquipRot);
                spawnedWeapon.gameObject.transform.localScale = spawnedWeapon.weaponEquipScale;

                spawnedWeapon.isUse = true;
                break;
            case WeaponType.Bow:
                spawnedWeapon = Instantiate(WeaponObjManager.Instance.bow, hand);

                spawnedWeapon.GetComponent<BoxCollider>().isTrigger = true;

                Destroy(spawnedWeapon.GetComponent<Rigidbody>());
                //weapon.GetComponent<Rigidbody>().useGravity = false;

                spawnedWeapon.transform.localPosition = spawnedWeapon.weaponEquipPos;
                spawnedWeapon.transform.localRotation = Quaternion.Euler(spawnedWeapon.weaponEquipRot);
                spawnedWeapon.gameObject.transform.localScale = spawnedWeapon.weaponEquipScale;

                spawnedWeapon.isUse = true;
                break;
            case WeaponType.Gun:
                spawnedWeapon = Instantiate(WeaponObjManager.Instance.gun, hand);

                spawnedWeapon.GetComponent<BoxCollider>().isTrigger = true;

                Destroy(spawnedWeapon.GetComponent<Rigidbody>());
                //weapon.GetComponent<Rigidbody>().useGravity = false;

                spawnedWeapon.transform.localPosition = spawnedWeapon.weaponEquipPos;
                spawnedWeapon.transform.localRotation = Quaternion.Euler(spawnedWeapon.weaponEquipRot);
                spawnedWeapon.gameObject.transform.localScale = spawnedWeapon.weaponEquipScale;

                spawnedWeapon.isUse = true;
                break;
            case WeaponType.ShotGun:
                spawnedWeapon = Instantiate(WeaponObjManager.Instance.shotGun, hand);

                spawnedWeapon.GetComponent<BoxCollider>().isTrigger = true;

                Destroy(spawnedWeapon.GetComponent<Rigidbody>());
                //weapon.GetComponent<Rigidbody>().useGravity = false;

                spawnedWeapon.transform.localPosition = spawnedWeapon.weaponEquipPos;
                spawnedWeapon.transform.localRotation = Quaternion.Euler(spawnedWeapon.weaponEquipRot);
                spawnedWeapon.gameObject.transform.localScale = spawnedWeapon.weaponEquipScale;

                spawnedWeapon.isUse = true;
                break;
            case WeaponType.Fire_Extinguisher:
                spawnedWeapon = Instantiate(WeaponObjManager.Instance.fireExtinguisher, hand);

                spawnedWeapon.GetComponent<BoxCollider>().isTrigger = true;

                Destroy(spawnedWeapon.GetComponent<Rigidbody>());
                //weapon.GetComponent<Rigidbody>().useGravity = false;

                spawnedWeapon.transform.localPosition = spawnedWeapon.weaponEquipPos;
                spawnedWeapon.transform.localRotation = Quaternion.Euler(spawnedWeapon.weaponEquipRot);
                spawnedWeapon.gameObject.transform.localScale = spawnedWeapon.weaponEquipScale;

                spawnedWeapon.isUse = true;
                break;
            case WeaponType.Lee_Shin:
                spawnedWeapon = Instantiate(WeaponObjManager.Instance.leeShin, head);

                spawnedWeapon.GetComponent<BoxCollider>().isTrigger = true;

                Destroy(spawnedWeapon.GetComponent<Rigidbody>());
                //weapon.GetComponent<Rigidbody>().useGravity = false;

                spawnedWeapon.transform.localPosition = spawnedWeapon.weaponEquipPos;
                spawnedWeapon.transform.localRotation = Quaternion.Euler(spawnedWeapon.weaponEquipRot);
                spawnedWeapon.gameObject.transform.localScale = spawnedWeapon.weaponEquipScale;

                spawnedWeapon.isUse = true;
                break;
            case WeaponType.Pica_Chu:
                spawnedWeapon = Instantiate(WeaponObjManager.Instance.picaChu, hand);

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
