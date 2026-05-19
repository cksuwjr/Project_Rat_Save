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

    NavMeshAgent nmAgent;

    CapsuleCollider capCollider;

    public override Vector3 GetDirection { get { return new Vector3(transform.forward.x, 0, transform.forward.z).normalized; } }


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
        while (target != null)
        {
            timer += Time.deltaTime;
            if (timer > 0.2f)
            {
                animator?.SetBool("Move", direction.sqrMagnitude > 0.01f);
                SetMoveTarget(target.transform.position);
                timer = 0f;
                nmAgent.isStopped = false;
            }

            if (target.CompareTag("Player") && Vector3.Distance(transform.position, target.transform.position) < weaponManager.weaponRange)
            {
                nmAgent.isStopped = true;
                GetComponent<Movement>().See(target.GetComponent<Entity>(), 4000);
                ChangeState(EnemyState.Attack);
            }
            if (target.CompareTag("Weapon") && Vector3.Distance(transform.position, target.transform.position) < 1.3f)
            {
                weaponManager.Fire(KeyInput.Fire5);
                movement?.Move(Vector3.zero, status.MoveSpeed);
                yield return YieldInstructionCache.WaitForSeconds(0.7f);
                target = GameManager.Instance.Player.gameObject;
                ChangeState(EnemyState.Chase);
            }

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



        if (other.CompareTag("Weapon") 
            && (!weaponManager.hand.GetComponentInChildren<WeaponObject>() && !weaponManager.head.GetComponentInChildren<WeaponObject>()))
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
        if (isDead) return;
        if (isHit) return;
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

        TryGetComponent<CapsuleCollider>(out capCollider);
    }

    public override void Init(float hp, float speed)
    {
        base.Init(hp, speed);

        weaponManager?.Init();
        StartAct();


        nmAgent.enabled = true;
        nmAgent.isStopped = false;
        nmAgent.ResetPath();

        capCollider.enabled = true;

        ChangeState(EnemyState.Patrol);

        OnChangeHp?.Invoke(status.HP, status.HP, status.MaxHP);

    }

    public override void GetDamage(Entity attacker, float damage, SkillType skillType, float knockbackTime = 3f, int effectNum = 0)
    {
        
        if (!hittable) return;

        var prevHp = status.HP;

        StopAllCoroutines();

        if (!isDead)
        {
            target = attacker.gameObject;
            ChangeState(EnemyState.Chase);
        }

        nmAgent.isStopped = true;

        if ((SkillType)enemyType != SkillType.Base && (SkillType)enemyType == skillType)
            base.GetDamage(this, damage * 0.05f, skillType, knockbackTime, effectNum);
        else
            base.GetDamage(this, damage, skillType, knockbackTime, effectNum);
        

        OnChangeHp?.Invoke(prevHp, status.HP, status.MaxHP);
        StartCoroutine("Invinsible");
    }


    protected override void Die()
    {
        base.Die();

        StopAllCoroutines();

        for (int i = 0; i < weaponManager.Skills.Length; i++)
            weaponManager.Skills[i]?.StopCast();

        if (TryGetComponent<Rigidbody>(out var rig))
            rig.velocity = Vector3.zero;

        transform.GetComponentInChildren<Animator>().SetTrigger("Die");


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

        treshWeapon = null;
        foreach (Transform weaponTr in weaponManager.head.GetComponentInChildren<Transform>())
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

        capCollider.enabled = false;


        if (nmAgent.enabled)
        {
            nmAgent.isStopped = true;
            nmAgent.ResetPath();
        }
        nmAgent.enabled = false;

        GameManager.Instance.Money++;
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

    public override void StopAct()
    {
        movement.Movable = false;
    }

    public override void StartAct()
    {
        if (isDead) return;
        if (isHit) return;

        movement.Movable = true;
        if(nmAgent.enabled) nmAgent.isStopped = false;
    }
}




