using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum EnemyType
{
    Base,
    Fire,
    Electronic,
}

public enum EnemyState
{
    Idle,
    Patrol,
    Chase,
    Attack,
}



public class EnemyController : Entity
{
    private IMove movement;
    private WeaponManager weaponManager;

    private bool hittable = true;

    private bool isBinded = false;

    private Vector3 InputVector;



    private EnemyState state;
    public int enemyType;

    private GameObject target;
    private Vector3 direction;

    public void ChangeState(EnemyState state)
    {
        StopCoroutine(this.state.ToString());
        this.state = state;
        StartCoroutine(this.state.ToString());
    }


    private IEnumerator Idle()
    {
        movement?.Move(Vector3.zero);


        float timer = 0f;
        while (timer < 3f)
        {
            yield return null;
            timer += Time.deltaTime;
        }
        ChangeState(EnemyState.Patrol);
    }

    private IEnumerator Patrol()
    {
        direction = UnityEngine.Random.insideUnitSphere;
        direction.y = 0;
        direction.Normalize();

        float timer = 0f;
        while (timer < 3f)
        {
            movement?.Move(direction);
            yield return null;
            timer += Time.deltaTime;
        }

        ChangeState(EnemyState.Idle);
    }

    private IEnumerator Chase()
    {
        float timer = 0f;

        SetMoveTarget(GameManager.Instance.Player.transform.position);

        while (target != null)
        {
            timer += Time.deltaTime;
            if (timer > 0.8f)
            {
                SetMoveTarget(GameManager.Instance.Player.transform.position);
                timer = 0f;
            }

            if (Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position) < 2)
                ChangeState(EnemyState.Attack);
            else
                movement?.Move(direction);
            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        movement?.Move(Vector3.zero);

        int num = UnityEngine.Random.Range(1, 3);

        switch (num)
        {
            case 1:
                weaponManager.Fire(KeyInput.Fire1);
                break;
            case 2:
                weaponManager.Fire(KeyInput.Fire2);
                break;
        }
        yield return YieldInstructionCache.WaitForSeconds(0.7f);
        ChangeState(EnemyState.Chase);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            target = other.gameObject;
            ChangeState(EnemyState.Chase);
        }
    }

    private void SetMoveTarget(Vector3 newPos)
    {
        var origin = direction;
        direction = newPos - transform.position;
        direction.y = 0;
        direction.Normalize();
        //transform.rotation = Quaternion.LookRotation(direction);
        
        StartCoroutine(ChangeDirection(origin, direction));

    }

    private IEnumerator ChangeDirection(Vector3 before, Vector3 after)
    {
        float timer = 0f;

        while (timer < 0.4f)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.Lerp(before, after, timer / 0.4f));
            //transform.rotation = Quaternion.LookRotation(direction);

            timer += Time.deltaTime;
            yield return null;
        }
    }




    protected override void DoAwake()
    {
        TryGetComponent<IMove>(out movement);

        TryGetComponent<WeaponManager>(out weaponManager);
        weaponManager?.Init();


        ChangeState(EnemyState.Idle);
    }

    public override void GetDamage(Entity attacker, float damage, SkillType skillType, float knockbackTime = 3f, int effectNum = 0)
    {
        if (!hittable) return;

        var prevHp = status.HP;

        if ((SkillType)enemyType != SkillType.Base && (SkillType)enemyType == skillType)
            base.GetDamage(this, damage * 0.05f, skillType, knockbackTime, effectNum);
        else
            base.GetDamage(this, damage, skillType, knockbackTime, effectNum);

        OnChangeHp?.Invoke(prevHp, status.HP, status.MaxHP);
        StartCoroutine("Invinsible");
    }


    protected override void Die()
    {
        GameManager.Instance.Player.GetComponentInChildren<Animator>().SetTrigger("Die");
        base.Die();
        StopAllCoroutines();
    }


    //private void Update()
    //{
    //    InputVector = GameManager.Instance.Player.transform.position - transform.position;

    //    InputVector.y = 0;
    //    InputVector.Normalize();



    //    if (!isBinded)
    //    {
    //        //    if (inputHandle.GetKeyInput(KeyInput.Fire1))
    //        //        weaponManager.Fire(KeyInput.Fire1);
    //        //    if (inputHandle.GetKeyInput(KeyInput.Fire2))
    //        //        weaponManager.Fire(KeyInput.Fire2);
    //        //    if (inputHandle.GetKeyInput(KeyInput.Fire3))
    //        //        weaponManager.Fire(KeyInput.Fire3);
    //        //    if (inputHandle.GetKeyInput(KeyInput.Fire4))
    //        //        weaponManager.Fire(KeyInput.Fire4);
    //        //    if (inputHandle.GetKeyInput(KeyInput.Fire5))
    //        //        weaponManager.Fire(KeyInput.Fire5);
    //        //}
    //    }
    //}

    //private void FixedUpdate()
    //{
    //    if (!isBinded)
    //    {
    //        movement?.Move(InputVector);
    //    }
    //}

    public void GetCC(float time)
    {
        StartCoroutine("CC", time);
    }

    public IEnumerator CC(float time)
    {
        isBinded = true;
        yield return YieldInstructionCache.WaitForSeconds(time);
        isBinded = false;
    }

    //public void Attack()
    //{

    //}


    public void GetHeal(float value)
    {
        var prevHp = status.HP;
        status.HP += value;
        if (status.HP >= status.MaxHP)
            status.HP = status.MaxHP;

        OnChangeHp?.Invoke(prevHp, status.HP, status.MaxHP);

    }


    private IEnumerator Invinsible()
    {
        //hittable = false;
        yield return null;
        hittable = true;
    }
}
