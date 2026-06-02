using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShot : Skill
{
    protected override IEnumerator Cast_()
    {
        controller.StopAct();

        animator?.SetBool("Move", false);

        if (weaponManager) weaponManager.attackable = false;

        float aimingTime = 0.9f;
        float timer = 0f;

        var move = GetComponent<Movement>();

        controller.OnChangeGage?.Invoke(0, 0.001f, aimingTime);
        while (Input.GetButton(skill_key.ToString()))
        {
            controller.OnChangeGage?.Invoke(timer, timer + Time.deltaTime, aimingTime);

            if (timer < aimingTime)
                timer += Time.deltaTime;
            else
            {
                var entity = controller.GetNearEnemy(weaponManager.weaponRange);
                if (entity) move.See(entity);
            }

            yield return null;
        }
        controller.OnChangeGage?.Invoke(0, 0.001f, aimingTime);

        animator?.SetTrigger("Fire1");

        var arrow = PoolManager.Instance.bulletPool.GetPoolObject();

        arrow.transform.position = weaponManager.left_Hand.transform.position;

        arrow.GetComponent<Projectile>().Init(controller, controller.GetDirection, damage + status.AttackPower * 0.8f, 2f);

        yield return YieldInstructionCache.WaitForSeconds(0.1f);

        if (weaponManager) weaponManager.attackable = true;

        controller.StartAct();

        yield return null;
    }
}
