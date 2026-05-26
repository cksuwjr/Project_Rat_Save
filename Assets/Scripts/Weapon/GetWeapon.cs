using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GetWeapon : Skill
{
    protected override IEnumerator Cast_()
    {
        animator?.SetTrigger("Fire2");

        controller.StopAct();

        if (weaponManager) weaponManager.attackable = false;

        yield return YieldInstructionCache.WaitForSeconds(0.35f);

        WeaponObject left_TreshWeapon = null;
        foreach(Transform weaponTr in weaponManager.left_Hand.GetComponentInChildren<Transform>())
        {
            if(weaponTr.gameObject.CompareTag("Weapon"))
            {
                if (weaponTr.TryGetComponent<WeaponObject>(out left_TreshWeapon))
                {
                    if (left_TreshWeapon.weaponType == WeaponType.Glove) break;


                    left_TreshWeapon.transform.SetParent(null);
                    left_TreshWeapon.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                    //treshWeapon.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    left_TreshWeapon.isUse = false;

                    left_TreshWeapon.AddComponent<Rigidbody>();
                    break;
                }
            }

        }

        WeaponObject right_TreshWeapon = null;
        foreach (Transform weaponTr in weaponManager.right_Hand.GetComponentInChildren<Transform>())
        {
            if (weaponTr.gameObject.CompareTag("Weapon"))
            {
                if (weaponTr.TryGetComponent<WeaponObject>(out right_TreshWeapon))
                {
                    if (right_TreshWeapon.weaponType == WeaponType.Glove) break;

                    right_TreshWeapon.transform.SetParent(null);
                    right_TreshWeapon.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                    //treshWeapon.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    right_TreshWeapon.isUse = false;

                    right_TreshWeapon.AddComponent<Rigidbody>();
                    break;
                }
            }
        }

        WeaponObject head_TreshWeapon = null;
        foreach (Transform weaponTr in weaponManager.head.GetComponentInChildren<Transform>())
        {
            if (weaponTr.gameObject.CompareTag("Weapon"))
            {
                if (weaponTr.TryGetComponent<WeaponObject>(out head_TreshWeapon))
                {
                    head_TreshWeapon.transform.SetParent(null);
                    head_TreshWeapon.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                    //treshWeapon.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    head_TreshWeapon.isUse = false;

                    head_TreshWeapon.AddComponent<Rigidbody>();
                    break;
                }
            }
        }

        Collider[] cols = Physics.OverlapSphere(transform.position, 1.4f);

        WeaponObject weapon = null;

        foreach (Collider col in cols)
        {
            if (col.gameObject.CompareTag("Weapon"))
            {
                if (col.gameObject == weaponManager.left_Hand.gameObject) continue;
                if (col.gameObject == weaponManager.right_Hand.gameObject) continue;
                if (col.gameObject == weaponManager.head.gameObject) continue;

                if (col.TryGetComponent<WeaponObject>(out weapon))
                {
                    if (left_TreshWeapon == weapon || right_TreshWeapon == weapon || head_TreshWeapon == weapon || weapon.isUse)
                    {
                        weapon = null;
                        continue;
                    }
                }
            }
        }

        if (weapon)
        {
            if (left_TreshWeapon != null && right_TreshWeapon != null)
            {
                left_TreshWeapon.transform.SetParent(null);
                left_TreshWeapon.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                //treshWeapon.gameObject.GetComponent<Rigidbody>().useGravity = true;
                left_TreshWeapon.isUse = false;

                left_TreshWeapon.AddComponent<Rigidbody>();

                right_TreshWeapon.transform.SetParent(null);
                right_TreshWeapon.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                //treshWeapon.gameObject.GetComponent<Rigidbody>().useGravity = true;
                right_TreshWeapon.isUse = false;

                right_TreshWeapon.AddComponent<Rigidbody>();

                Debug.Log("À¸¾î");
            }
            else
            {
                if (left_TreshWeapon != null)
                    if (weapon.weaponType != WeaponType.Glove && left_TreshWeapon.weaponType == WeaponType.Glove)
                    {
                        left_TreshWeapon.transform.SetParent(null);
                        left_TreshWeapon.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                        //treshWeapon.gameObject.GetComponent<Rigidbody>().useGravity = true;
                        left_TreshWeapon.isUse = false;

                        left_TreshWeapon.AddComponent<Rigidbody>();
                    }

                if (right_TreshWeapon != null)
                    if (weapon.weaponType != WeaponType.Glove && right_TreshWeapon.weaponType == WeaponType.Glove)
                    {
                        right_TreshWeapon.transform.SetParent(null);
                        right_TreshWeapon.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                        //treshWeapon.gameObject.GetComponent<Rigidbody>().useGravity = true;
                        right_TreshWeapon.isUse = false;

                        right_TreshWeapon.AddComponent<Rigidbody>();
                    }
            }
            


            weapon.GetComponent<BoxCollider>().isTrigger = true;

            Destroy(weapon.GetComponent<Rigidbody>());
            //weapon.GetComponent<Rigidbody>().useGravity = false;

            if (weapon.equipType == WeaponEquipType.Left_Hand) weapon.transform.SetParent(weaponManager.left_Hand.transform);
            if (weapon.equipType == WeaponEquipType.Right_Hand) weapon.transform.SetParent(weaponManager.right_Hand.transform);
            if (weapon.equipType == WeaponEquipType.Head) weapon.transform.SetParent(weaponManager.head.transform);

            weapon.transform.localPosition = weapon.weaponEquipPos;
            weapon.transform.localRotation = Quaternion.Euler(weapon.weaponEquipRot);
            weapon.gameObject.transform.localScale = weapon.weaponEquipScale;

            weapon.isUse = true;
            weapon.GetWeaponEvent?.Invoke();
            weapon.GetWeaponEvent = null;

            weaponManager.ChangeWeapon(weapon.weaponType);
        }
        else
            weaponManager.ChangeWeapon(WeaponType.Hand);

        yield return YieldInstructionCache.WaitForSeconds(0.35f);

        if (weaponManager) weaponManager.attackable = true;

        controller.StartAct();


        yield return null;
    }
}
