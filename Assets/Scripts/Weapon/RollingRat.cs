using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingRat : Skill
{
    protected override IEnumerator Cast_()
    {
        animator?.SetTrigger("Fire3");

        if (weaponManager) weaponManager.attackable = false;

        controller.StartCoroutine("Invinsible");
        yield return YieldInstructionCache.WaitForSeconds(0.15f);

        PlayerMove move;

        float timer = 0;
        while (timer < 0.28f)
        {
            if (TryGetComponent<PlayerMove>(out move))
                move.Rolling(15);
            yield return YieldInstructionCache.waitForFixedUpdate;
            timer += Time.fixedDeltaTime;
        }
        yield return YieldInstructionCache.WaitForSeconds(0.27f);


        if (weaponManager) weaponManager.attackable = true;

        yield return null;
    }
}
