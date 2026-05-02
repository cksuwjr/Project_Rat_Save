using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemyElimentalType
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

public enum EnemyType
{
    OrangeCat,
    BlackCat,
}


public class EnemyController1 : Entity
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

    NavMeshAgent nmAgent;

    private void Start()
    {
        ChangeState(EnemyState.Idle);
    }

    public void ChangeState(EnemyState state)
    {
        StopCoroutine(this.state.ToString());
        this.state = state;
        StartCoroutine(this.state.ToString());
    }


    private IEnumerator Idle()
    {
        direction = transform.position;
        SetMoveTarget(direction);

        //movement?.Move(Vector3.zero, status.MoveSpeed);
        Animator animator = GetComponentInChildren<Animator>();
        animator?.SetBool("Move", false);

        float timer = 0f;
        float maxTimer = Random.Range(0f, 3f);
        while (timer < maxTimer)
        {
            yield return null;
            timer += Time.deltaTime;
        }
        ChangeState(EnemyState.Patrol);
    }

    private IEnumerator Patrol()
    {
        float timer = 0f;
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Animator animator = GetComponentInChildren<Animator>();


        float maxTimer = Random.Range(3f, 7f);
        while (timer < maxTimer)
        {
            if (!agent.hasPath || agent.remainingDistance < 0.2f)
            {
                Vector2 random = UnityEngine.Random.insideUnitCircle * 2f;
                Vector3 target = transform.position + (new Vector3(random.x, 0, random.y) * maxTimer * 10);

                NavMeshHit hit;
                if (NavMesh.SamplePosition(target, out hit, 2f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
            }

            animator?.SetBool("Move", agent.velocity.sqrMagnitude > 0.01f);

            yield return null;
            timer += Time.deltaTime;
        }
        ChangeState(EnemyState.Idle);
    }

    private IEnumerator Chase()
    {
        float timer = 0f;
        Animator animator = GetComponentInChildren<Animator>();
        //SetMoveTarget(target.transform.position);
        Debug.Log("추격중이니?");
        while (target != null)
        {
            timer += Time.deltaTime;
            if (timer > 0.2f)
            {
                animator?.SetBool("Move", direction.sqrMagnitude > 0.01f);
                SetMoveTarget(target.transform.position);
                timer = 0f;
            }
            bool flag = false;

            if (target.CompareTag("Player") && Vector3.Distance(transform.position, target.transform.position) < 2)
            {
                ChangeState(EnemyState.Attack);
                flag = true;
            }

            if (target.CompareTag("Weapon") && Vector3.Distance(transform.position, target.transform.position) < 1.3f)
            {
                weaponManager.Fire(KeyInput.Fire5);
                movement?.Move(Vector3.zero, status.MoveSpeed);
                yield return YieldInstructionCache.WaitForSeconds(0.7f);
                target = GameManager.Instance.Player.gameObject;
                ChangeState(EnemyState.Chase);
                flag = true;
            }

            //if(!flag)
            //    movement?.Move(direction, status.MoveSpeed);

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        movement?.Move(Vector3.zero, status.MoveSpeed);

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



        if (other.CompareTag("Weapon") && !weaponManager.hand.GetComponentInChildren<WeaponObject>())
        {
            WeaponObject weapon;
            if (other.TryGetComponent<WeaponObject>(out weapon))
            {
                if (!weapon.isUse)
                {
                    StopAllCoroutines();
                    target = other.gameObject;
                    ChangeState(EnemyState.Chase);
                    return;
                }
            }
        }

        if (other.CompareTag("Player"))
        {
            StopAllCoroutines();
            target = other.gameObject;
            ChangeState(EnemyState.Chase);
            return;
        }

    }

    private void SetMoveTarget(Vector3 newPos)
    {
        //var origin = direction;
        //direction = newPos - transform.position;
        //direction.y = 0;
        //direction.Normalize();
        //transform.rotation = Quaternion.LookRotation(direction);
        direction = newPos;
        nmAgent.SetDestination(newPos);

        //StartCoroutine(ChangeDirection(origin, direction));

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

        TryGetComponent<NavMeshAgent>(out nmAgent);
    }

    public override void Init(float hp, float speed)
    {
        base.Init(hp, speed);

        weaponManager?.Init();

        ChangeState(EnemyState.Idle);

        OnChangeHp?.Invoke(status.HP, status.HP, status.MaxHP);
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
        transform.GetComponentInChildren<Animator>().SetTrigger("Die");
        base.Die();
        StopAllCoroutines();

        if (!weaponManager.hand) return;

        WeaponObject treshWeapon = null;
        foreach (Transform weaponTr in weaponManager.hand.GetComponentInChildren<Transform>())
        {
            if (weaponTr.gameObject.CompareTag("Weapon"))
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
    }

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






//using System;
//using System.Collections;
//using Unity.VisualScripting;
//using UnityEngine;



//public enum EnemyElimentalType
//{
//    Base,
//    Fire,
//    Electronic,
//}

//public enum EnemyState
//{
//    Idle,
//    Patrol,
//    Chase,
//    Attack,
//}

//public enum EnemyType
//{
//    OrangeCat,
//    BlackCat,
//}



//public class EnemyController : Entity
//{
//    private IMove movement;
//    private WeaponManager weaponManager;

//    private bool hittable = true;

//    private bool isBinded = false;

//    private Vector3 InputVector;



//    private EnemyState state;
//    public int enemyType;

//    private GameObject target;
//    private Vector3 direction;

//    public void ChangeState(EnemyState state)
//    {
//        StopCoroutine(this.state.ToString());
//        this.state = state;
//        StartCoroutine(this.state.ToString());
//    }


//    private IEnumerator Idle()
//    {
//        movement?.Move(Vector3.zero, status.MoveSpeed);


//        float timer = 0f;
//        while (timer < 3f)
//        {
//            yield return null;
//            timer += Time.deltaTime;
//        }
//        ChangeState(EnemyState.Patrol);
//    }

//    private IEnumerator Patrol()
//    {
//        direction = UnityEngine.Random.insideUnitSphere;
//        direction.y = 0;
//        direction.Normalize();

//        float timer = 0f;
//        while (timer < 3f)
//        {
//            movement?.Move(direction, status.MoveSpeed);
//            yield return null;
//            timer += Time.deltaTime;
//        }

//        ChangeState(EnemyState.Idle);
//    }

//    private IEnumerator Chase()
//    {
//        float timer = 0f;

//        SetMoveTarget(target.transform.position);

//        while (target != null)
//        {
//            timer += Time.deltaTime;
//            if (timer > 0.8f)
//            {
//                SetMoveTarget(target.transform.position);
//                timer = 0f;
//            }
//            bool flag = false;

//            if (target.CompareTag("Player") && Vector3.Distance(transform.position, target.transform.position) < 2)
//            {
//                ChangeState(EnemyState.Attack);
//                flag = true;
//            }
            
//            if (target.CompareTag("Weapon") && Vector3.Distance(transform.position, target.transform.position) < 1.3f)
//            {
//                weaponManager.Fire(KeyInput.Fire5);
//                movement?.Move(Vector3.zero, status.MoveSpeed);
//                yield return YieldInstructionCache.WaitForSeconds(0.7f);
//                target = GameManager.Instance.Player.gameObject;
//                ChangeState(EnemyState.Chase);
//                flag = true;
//            }
            
//            if(!flag)
//                movement?.Move(direction, status.MoveSpeed);
//            yield return null;
//        }
//    }

//    private IEnumerator Attack()
//    {
//        movement?.Move(Vector3.zero, status.MoveSpeed);

//        int num = UnityEngine.Random.Range(1, 3);

//        switch (num)
//        {
//            case 1:
//                weaponManager.Fire(KeyInput.Fire1);
//                break;
//            case 2:
//                weaponManager.Fire(KeyInput.Fire2);
//                break;
//        }
//        yield return YieldInstructionCache.WaitForSeconds(0.7f);
//        ChangeState(EnemyState.Chase);
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (isDead) return;

        
        
//        if(other.CompareTag("Weapon") && !weaponManager.hand.GetComponentInChildren<WeaponObject>())
//        {
//            WeaponObject weapon;
//            if (other.TryGetComponent<WeaponObject>(out weapon))
//            {
//                if(!weapon.isUse)
//                {
//                    StopAllCoroutines();
//                    target = other.gameObject;
//                    ChangeState(EnemyState.Chase);
//                    return;
//                }
//            }
//        }
        
//        if (other.CompareTag("Player"))
//        {
//            StopAllCoroutines();
//            target = other.gameObject;
//            ChangeState(EnemyState.Chase);
//            return;
//        }

//    }

//    private void SetMoveTarget(Vector3 newPos)
//    {
//        var origin = direction;
//        direction = newPos - transform.position;
//        direction.y = 0;
//        direction.Normalize();
//        //transform.rotation = Quaternion.LookRotation(direction);
        
//        StartCoroutine(ChangeDirection(origin, direction));

//    }

//    private IEnumerator ChangeDirection(Vector3 before, Vector3 after)
//    {
//        float timer = 0f;

//        while (timer < 0.4f)
//        {
//            transform.rotation = Quaternion.LookRotation(Vector3.Lerp(before, after, timer / 0.4f));
//            //transform.rotation = Quaternion.LookRotation(direction);

//            timer += Time.deltaTime;
//            yield return null;
//        }
//    }




//    protected override void DoAwake()
//    {
//        TryGetComponent<IMove>(out movement);

//        TryGetComponent<WeaponManager>(out weaponManager);

//    }

//    public override void Init(float hp, float speed)
//    {
//        base.Init(hp, speed);

//        weaponManager?.Init();

//        ChangeState(EnemyState.Idle);

//        OnChangeHp?.Invoke(status.HP, status.HP, status.MaxHP);
//    }

//    public override void GetDamage(Entity attacker, float damage, SkillType skillType, float knockbackTime = 3f, int effectNum = 0)
//    {
//        if (!hittable) return;

//        var prevHp = status.HP;

//        if ((SkillType)enemyType != SkillType.Base && (SkillType)enemyType == skillType)
//            base.GetDamage(this, damage * 0.05f, skillType, knockbackTime, effectNum);
//        else
//            base.GetDamage(this, damage, skillType, knockbackTime, effectNum);

//        OnChangeHp?.Invoke(prevHp, status.HP, status.MaxHP);
//        StartCoroutine("Invinsible");
//    }


//    protected override void Die()
//    {
//        transform.GetComponentInChildren<Animator>().SetTrigger("Die");
//        base.Die();
//        StopAllCoroutines();

//        if (!weaponManager.hand) return;

//        WeaponObject treshWeapon = null;
//        foreach (Transform weaponTr in weaponManager.hand.GetComponentInChildren<Transform>())
//        {
//            if (weaponTr.gameObject.CompareTag("Weapon"))
//            {
//                if (weaponTr.TryGetComponent<WeaponObject>(out treshWeapon))
//                {
//                    treshWeapon.transform.SetParent(null);
//                    treshWeapon.gameObject.GetComponent<BoxCollider>().isTrigger = false;
//                    //treshWeapon.gameObject.GetComponent<Rigidbody>().useGravity = true;
//                    treshWeapon.isUse = false;

//                    treshWeapon.AddComponent<Rigidbody>();
//                    break;
//                }
//            }
//        }
//    }

//    public void GetCC(float time)
//    {
//        StartCoroutine("CC", time);
//    }

//    public IEnumerator CC(float time)
//    {
//        isBinded = true;
//        yield return YieldInstructionCache.WaitForSeconds(time);
//        isBinded = false;
//    }

//    public void GetHeal(float value)
//    {
//        var prevHp = status.HP;
//        status.HP += value;
//        if (status.HP >= status.MaxHP)
//            status.HP = status.MaxHP;

//        OnChangeHp?.Invoke(prevHp, status.HP, status.MaxHP);

//    }

//    private IEnumerator Invinsible()
//    {
//        //hittable = false;
//        yield return null;
//        hittable = true;
//    }
//}
