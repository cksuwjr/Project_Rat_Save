using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingShoot : Skill
{
    protected override IEnumerator Cast_()
    {
        controller.StopAct();

        
        if (weaponManager) weaponManager.attackable = false;

        animator?.SetBool("Move", false);

        float aimingTime = 0.15f;
        float timer = 0f;

        var move = GetComponent<Movement>();

        controller.OnChangeGage?.Invoke(0, 0.001f, aimingTime);
        while (Input.GetButton(skill_key.ToString()))
        {
            if (timer < aimingTime)
                timer += Time.deltaTime;
            else
            {
                var entity = controller.GetNearEnemy(weaponManager.weaponRange);
                if (entity) move.See(entity);
            }
            controller.OnChangeGage?.Invoke(timer, timer, aimingTime);
            yield return null;
        }
        controller.OnChangeGage?.Invoke(0, 0.001f, aimingTime);

        animator?.SetTrigger("Fire2");

        timer = 0;
        while (timer < 0.14f)
        {
            if (TryGetComponent<Movement>(out move))
                move.Rolling(-30);
            yield return YieldInstructionCache.waitForFixedUpdate;
            timer += Time.fixedDeltaTime;
        }

        //animator?.SetTrigger("Fire1");

        
        for (int i = 0; i < 3; i++)
        {
            var arrow = PoolManager.Instance.arrowPool.GetPoolObject();

            arrow.transform.position = transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));
            arrow.GetComponent<Projectile>().Init(controller, controller.GetDirection, damage + status.AttackPower * 0.5f, 22f);
            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
        yield return YieldInstructionCache.WaitForSeconds(0.1f);

        if (weaponManager) weaponManager.attackable = true;

        controller.StartAct();

        yield return null;
    }
}