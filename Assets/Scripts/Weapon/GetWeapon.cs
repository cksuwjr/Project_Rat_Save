using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GetWeapon : Skill
{
    protected override IEnumerator Cast_()
    {
        animator?.SetTrigger("Fire2");

        if (weaponManager) weaponManager.attackable = false;

        yield return YieldInstructionCache.WaitForSeconds(0.35f);

        WeaponObject treshWeapon = null;
        foreach(Transform weaponTr in weaponManager.hand.GetComponentInChildren<Transform>())
        {
            if(weaponTr.gameObject.CompareTag("Weapon"))
            {
                if (weaponTr.TryGetComponent<WeaponObject>(out treshWeapon))
                {
                    treshWeapon.transform.SetParent(null);
                    treshWeapon.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                    //treshWeapon.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    treshWeapon.isUse = false;

                    treshWeapon.AddComponent<Rigidbody>();
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
                if (col.gameObject == weaponManager.hand.gameObject) continue;
                if (col.TryGetComponent<WeaponObject>(out weapon))
                    if (treshWeapon == weapon || weapon.isUse)
                    {
                        weapon = null;
                        continue;
                    }
            }
        }

        if (weapon)
        {
            weapon.GetComponent<BoxCollider>().isTrigger = true;

            Destroy(weapon.GetComponent<Rigidbody>());
            //weapon.GetComponent<Rigidbody>().useGravity = false;

            weapon.transform.SetParent(weaponManager.hand.transform);
            weapon.transform.localPosition = weapon.weaponEquipPos;
            weapon.transform.localRotation = Quaternion.Euler(weapon.weaponEquipRot);
            weapon.gameObject.transform.localScale = weapon.weaponEquipScale;

            weapon.isUse = true;

            weaponManager.ChangeWeapon(weapon.weaponType);
        }
        else
            weaponManager.ChangeWeapon(WeaponType.Hand);

        yield return YieldInstructionCache.WaitForSeconds(0.35f);

        if (weaponManager) weaponManager.attackable = true;

        yield return null;
    }
}
