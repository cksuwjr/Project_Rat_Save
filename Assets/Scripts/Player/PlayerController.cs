using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerController : Entity
{
    private IMove movement;
    private IInputHandle inputHandle;
    private WeaponManager weaponManager;

    private bool hittable = true;

    

    private bool isBinded = false;

    private Vector3 InputVector;


    protected override void DoAwake()
    {
        TryGetComponent<IMove>(out movement);
        TryGetComponent<IInputHandle>(out inputHandle);

        TryGetComponent<WeaponManager>(out weaponManager);
        weaponManager?.Init();
    }

    public override void GetDamage(Entity attacker, float damage, float knockbackTime = 3f, int effectNum = 0)
    {
        if (!hittable) return;

        var prevHp = status.HP;
        base.GetDamage(this, damage, knockbackTime, effectNum);

        OnChangeHp?.Invoke(prevHp, status.HP, status.MaxHP);
        StartCoroutine("Invinsible");
    }


    protected override void Die()
    {
        GameManager.Instance.Player.GetComponentInChildren<Animator>().SetTrigger("Die");
        base.Die();
    }


    private void Update()
    {
        InputVector = inputHandle.GetInput();

        if (!isBinded)
        {
            if (inputHandle.GetKeyInput(KeyInput.Fire1))
                weaponManager.Fire(KeyInput.Fire1);
            if (inputHandle.GetKeyInput(KeyInput.Fire2))
                weaponManager.Fire(KeyInput.Fire2);
            if (inputHandle.GetKeyInput(KeyInput.Fire3))
                weaponManager.Fire(KeyInput.Fire3);
            if (inputHandle.GetKeyInput(KeyInput.Fire4))
                weaponManager.Fire(KeyInput.Fire4);
            if (inputHandle.GetKeyInput(KeyInput.Fire5))
                weaponManager.Fire(KeyInput.Fire5);
        }
    }

    private void FixedUpdate()
    {
        if (!isBinded)
        {
            movement?.Move(InputVector);
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
        yield return YieldInstructionCache.WaitForSeconds(0.5f);
        hittable = true;
    }
}