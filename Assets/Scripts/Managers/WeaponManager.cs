using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private bool fire1;
    private bool fire2;
    private bool fire3;
    private bool fire4;
    private bool fire5;

    [SerializeField] private Skill basicAttack;

    [SerializeField] private Skill skill1;
    [SerializeField] private Skill skill2;
    [SerializeField] private Skill skill3;
    [SerializeField] private Skill skill4;

    [SerializeField] private Skill specialSkill;

    private void Awake()
    {
        basicAttack.SetAuto(true);
    }

    public void Init()
    {
        basicAttack.Init();

        skill1.Init();
        skill2.Init();
        skill3.Init();
        skill4.Init();

        specialSkill.Init();
    }


    public void Fire(KeyInput input)
    {
        switch (input)
        {
            case KeyInput.Fire1: fire1 = true; break;
            case KeyInput.Fire2: fire2 = true; break;
            case KeyInput.Fire3: fire3 = true; break;
            case KeyInput.Fire4: fire4 = true; break;

            case KeyInput.Fire5: fire5 = true; break;
        }
    }

    private void FixedUpdate()
    {
        if (fire1)
        {
            Skill1();
            fire1 = false;
        }
        if (fire2)
        {
            Skill2();
            fire2 = false;
        }
        if (fire3)
        {
            Skill3();
            fire3 = false;
        }
        if (fire4)
        {
            Skill4();
            fire4 = false;
        }

        if (fire5)
        {
            Skill5();
            fire5 = false;
        }
    }

    private void Skill1()
    {
        skill1.Cast();

        Debug.Log("스킬1 사용");
    }

    private void Skill2()
    {
        skill2.Cast();

        Debug.Log("스킬2 사용");
    }

    private void Skill3()
    {
        skill3.Cast();

        Debug.Log("스킬3 사용");
    }

    private void Skill4()
    {
        skill4.Cast();

        Debug.Log("스킬4 사용");
    }

    private void Skill5()
    {
        specialSkill.Cast();

        Debug.Log("스킬5 사용");
    }

    public void AllCoolTimeDecline(float time)
    {
        basicAttack.CooltimeDecline(time);

        if (skill1.skill_Level > 0) skill1.CooltimeDecline(time);
        if (skill2.skill_Level > 0) skill2.CooltimeDecline(time);
        if (skill3.skill_Level > 0) skill3.CooltimeDecline(time);
        if (skill4.skill_Level > 0) skill4.CooltimeDecline(time);
    }

    public Skill GetSkillBasic()
    {
        return basicAttack;
    }

    public Skill GetSkillQ()
    {
        return skill1;
    }
    public Skill GetSkillW()
    {
        return skill2;
    }
    public Skill GetSkillE()
    {
        return skill3;
    }
    public Skill GetSkillR()
    {
        return skill4;
    }
}