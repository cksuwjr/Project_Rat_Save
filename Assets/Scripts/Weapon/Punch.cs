using System.Collections;
using UnityEngine;

public class Punch : Skill
{
    protected override IEnumerator Cast_()
    {
        animator?.SetTrigger("Fire1");
        if (weaponManager) weaponManager.attackable = false;

        yield return YieldInstructionCache.WaitForSeconds(0.1f);

        Collider[] cols = Physics.OverlapSphere(transform.position, 2);

        Entity enemy = null;

        foreach (Collider col in cols)
        {
            if (col.isTrigger) continue;

            if ((controller.enemyLayer & (1 << col.gameObject.layer)) != 0)
            {
                if (col.GetComponent<Entity>())
                    if (col.GetComponent<Entity>().isDead) continue;

                if (enemy)
                {
                    if (Vector3.Distance(enemy.transform.position, transform.position) > Vector3.Distance(col.transform.position, transform.position))
                    {
                        col.TryGetComponent<Entity>(out enemy);
                    }
                }
                else
                    col.TryGetComponent<Entity>(out enemy);
            }
        }

        enemy?.GetDamage(controller, damage, skill_Type, 0.5f, 2);

        yield return YieldInstructionCache.WaitForSeconds(0.25f);

        if (weaponManager) weaponManager.attackable = true;

        yield return null;
    }
}
