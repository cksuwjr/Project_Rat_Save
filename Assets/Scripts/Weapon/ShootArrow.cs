using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrow : Skill
{
    protected override IEnumerator Cast_()
    {
        controller.StopAct();

        animator?.SetTrigger("Fire1");
        if (weaponManager) weaponManager.attackable = false;

        yield return YieldInstructionCache.WaitForSeconds(0.1f);


        var arrow = PoolManager.Instance.arrowPool.GetPoolObject();

        arrow.transform.position = weaponManager.hand.transform.position;
        arrow.GetComponent<Projectile>().Init(controller, controller.GetDirection, damage + status.AttackPower * 0.3f, 22f);

        yield return YieldInstructionCache.WaitForSeconds(0.1f);

        if (weaponManager) weaponManager.attackable = true;

        controller.StartAct();

        yield return null;
    }
}
