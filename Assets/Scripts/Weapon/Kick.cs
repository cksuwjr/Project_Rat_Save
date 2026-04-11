using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick : Skill
{
    protected override IEnumerator Cast_()
    {
        animator?.SetTrigger("Fire2");

        if(weaponManager) weaponManager.attackable = false;

        yield return YieldInstructionCache.WaitForSeconds(0.35f);

        Collider[] cols = Physics.OverlapSphere(transform.position, 2);

        foreach(Collider col in cols)
        {
            if (col.isTrigger) continue;

            if ((controller.enemyLayer & (1 << col.gameObject.layer)) != 0)
            {
                Entity enemy;
                col.TryGetComponent<Entity>(out enemy);
                enemy?.GetDamage(controller, damage, skill_Type, 0.5f, 3);
            }
        }
        yield return YieldInstructionCache.WaitForSeconds(0.35f);

        if (weaponManager) weaponManager.attackable = true;

        yield return null;
    }
}
