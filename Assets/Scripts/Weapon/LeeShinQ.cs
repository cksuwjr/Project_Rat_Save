using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeeShinQ : Skill
{
    public bool IsQHit = false;
    public GameObject QTarget;
    public float OnHitTime;

    protected override IEnumerator Cast_()
    {
        controller.StopAct();

        if (weaponManager) weaponManager.attackable = false;

        bool QAfter = true;

        if(!IsQHit) QAfter = false;
        if(!QTarget) QAfter = false;
        else if (QTarget.GetComponent<Entity>().isDead) QAfter = false;


        if (!QAfter)
        {

            float aimingTime = 0.15f;
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



            animator?.SetTrigger("Fire2");


            var leeshinQ = PoolManager.Instance.leeshinQPool.GetPoolObject();

            

            leeshinQ.transform.position = weaponManager.left_Hand.transform.position;

            leeshinQ.GetComponent<Projectile>().Init(controller, controller.GetDirection, damage + status.AttackPower * 0.3f, 22f, 0.5f);
            leeshinQ.GetComponent<Projectile>().OnHitEvent = null;
            leeshinQ.GetComponent<Projectile>().OnHitEvent += (hit) => { IsQHit = true; QTarget = hit; OnHitTime = Time.time; };

            yield return YieldInstructionCache.WaitForSeconds(0.1f);
        }
        else
        {
            var startPos = transform.position;
            var endPos = QTarget.transform.position;
            endPos.y = startPos.y;

            float timer = 0f;
            float arriveTime;

            var rb = GetComponent<Rigidbody>();

            arriveTime = Vector3.Distance(startPos, endPos) / 20f;

            while (timer < arriveTime)
            {                                                              
                rb.position = Vector3.Lerp(transform.position, endPos, timer / arriveTime);
                timer += Time.deltaTime;
                yield return null;
            }

            var enemy = QTarget.GetComponent<Entity>();
            enemy?.GetDamage(controller, damage * 2.7f + status.AttackPower * 2.7f, skill_Type, 0.5f, 2);

            IsQHit = false;
            QTarget = null;
}

        if (weaponManager) weaponManager.attackable = true;

        controller.StartAct();

        yield return null;
    }
}
