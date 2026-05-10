using System;
using System.Collections;
using UnityEngine;

public class PlayerController : Entity
{
    private IMove movement;
    private IInputHandle inputHandle;
    private WeaponManager weaponManager;

    private bool hittable = true;



    private Vector3 InputVector;
    private Vector3 Input3DVector;
    public override Vector3 GetDirection { get {  return movement.Direction; } }
    

    protected override void DoAwake()
    {
        TryGetComponent<IMove>(out movement);
        TryGetComponent<IInputHandle>(out inputHandle);

        TryGetComponent<WeaponManager>(out weaponManager);
    }

    public override void Init(float hp, float speed)
    {
        base.Init(hp, speed);

        weaponManager?.Init();

        OnChangeHp?.Invoke(status.HP, status.HP, status.MaxHP);
    }

    public override void GetDamage(Entity attacker, float damage, SkillType skillType, float knockbackTime = 3f, int effectNum = 0)
    {
        if (!hittable) return;

        var prevHp = status.HP;
        base.GetDamage(this, damage, skillType, knockbackTime, effectNum);

        OnChangeHp?.Invoke(prevHp, status.HP, status.MaxHP);
        //StartCoroutine("Invinsible");
    }


    protected override void Die()
    {
        if (isDead) return;

        transform.GetComponentInChildren<Animator>().SetTrigger("Die");
        base.Die();
        movement.Movable = false;
        StopAllCoroutines();
    }


    private void Update()
    {
        InputVector = inputHandle.GetInput();


        


        var camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        var camRight = Camera.main.transform.right;
        camRight.y = 0f;

        Input3DVector = InputVector.z * camForward + InputVector.x * camRight;



        if (movement.Movable)
        {
            if (inputHandle.GetKeyInput(KeyInput.Fire1))
                weaponManager.Fire(KeyInput.Fire1);
            if (inputHandle.GetKeyInput(KeyInput.Fire2))
                weaponManager.Fire(KeyInput.Fire2);
            //if (inputHandle.GetKeyInput(KeyInput.Fire3))
            //    weaponManager.Fire(KeyInput.Fire3);
            if (inputHandle.GetKeyInput(KeyInput.Fire4))
                weaponManager.Fire(KeyInput.Fire4);
            if (inputHandle.GetKeyInput(KeyInput.Fire5))
                weaponManager.Fire(KeyInput.Fire5);
        }
        if (inputHandle.GetKeyInput(KeyInput.Fire3))
            weaponManager.Fire(KeyInput.Fire3);
    }

    private void FixedUpdate()
    {
        if (CameraManager.cameraMode != CameraMode.CAM3)
            movement?.Move(InputVector.normalized, status.MoveSpeed);
        else
            movement?.Move(Input3DVector.normalized, status.MoveSpeed);
    }

    public void GetCC(float time)
    {
        StartCoroutine("CC", time);
    }

    public IEnumerator CC(float time)
    {
        movement.Movable = false;
        yield return YieldInstructionCache.WaitForSeconds(time);
        movement.Movable = true;
    }

    public void Attack()
    {

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
        hittable = false;
        yield return YieldInstructionCache.WaitForSeconds(0.7f);
        hittable = true;
    }

    public override void StopAct() 
    {
        movement.Movable = false;
    }

    public override void StartAct() 
    {
        if (isDead) return;

        movement.Movable = true;
    }
}