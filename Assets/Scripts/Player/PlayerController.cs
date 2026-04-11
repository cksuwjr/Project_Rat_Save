using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

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

    public override void GetDamage(Entity attacker, float damage, SkillType skillType, float knockbackTime = 3f, int effectNum = 0)
    {
        if (!hittable) return;

        var prevHp = status.HP;
        base.GetDamage(this, damage, skillType, knockbackTime, effectNum);

        OnChangeHp?.Invoke(prevHp, status.HP, status.MaxHP);
        StartCoroutine("Invinsible");
    }


    protected override void Die()
    {
        if (isDead) return;

        transform.GetComponentInChildren<Animator>().SetTrigger("Die");
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


            if (Input.GetKeyDown(KeyCode.F1))
            {
                if (Camera.main.orthographic) return;
                //Camera.main.transform.GetComponent<CameraMove>().SetDist();
                Camera.main.orthographic = true;
                Camera.main.orthographicSize = 5.35f;
                Camera.main.GetComponent<CameraMove>().AddDist(new Vector3(0, -2.26f, 1.95f));

            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                if (!Camera.main.orthographic) return;

                Camera.main.orthographic = false;
                Camera.main.fieldOfView = 60;
                Camera.main.GetComponent<CameraMove>().AddDist(new Vector3(0, 2.26f, -1.95f));

            }
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