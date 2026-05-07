using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrow : Skill
{
    protected override IEnumerator Cast_()
    {
        controller.StopAct();

        animator?.SetBool("Move", false);

        if (weaponManager) weaponManager.attackable = false;

        float aimingTime = 0.15f;
        float timer = 0f;

        var move = GetComponent<Movement>();

        while(Input.GetButton(skill_key.ToString()))
        {
            if (timer < aimingTime)
                timer += Time.deltaTime;
            else
            {
                var entity = controller.GetNearEnemy(10);
                if(entity) move.See(entity);
            }
            yield return null;
        }

        animator?.SetTrigger("Fire1");

        var arrow = PoolManager.Instance.arrowPool.GetPoolObject();

        arrow.transform.position = weaponManager.hand.transform.position;

        arrow.GetComponent<Projectile>().Init(controller, controller.GetDirection, damage + status.AttackPower * 0.3f, 22f);

        yield return YieldInstructionCache.WaitForSeconds(0.1f);

        if (weaponManager) weaponManager.attackable = true;

        controller.StartAct();

        yield return null;
    }
}
