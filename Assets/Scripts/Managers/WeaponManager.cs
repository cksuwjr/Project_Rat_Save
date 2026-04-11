using UnityEngine;

public enum WeaponType
{
    Hand,
    Wood_Carving,
}

public class WeaponManager : MonoBehaviour
{
    private WeaponType weaponType = 0;
    public Transform hand;

    private bool fire1;
    private bool fire2;
    private bool fire3;
    private bool fire4;
    private bool fire5;

    [SerializeField] private Skill skill1;
    [SerializeField] private Skill skill2;
    [SerializeField] private Skill skill3;
    [SerializeField] private Skill skill4;
    [SerializeField] private Skill skill5;

    public bool attackable = true;

    private void Awake()
    {
        
    }

    public void Init()
    {
        var weapon = hand.GetComponentInChildren<WeaponObject>();
        if (weapon)
            ChangeWeapon(weapon.weaponType);
        else
            ChangeWeapon(WeaponType.Hand);
    }

    public void ChangeWeapon(WeaponType weapon)
    {
        if (skill1) Destroy(skill1);
        if (skill2) Destroy(skill2);
        if (skill3) Destroy(skill3);
        if (skill4) Destroy(skill4);
        // if (skill5) Destroy(skill5); // change weapon during Destroy have Trouble

        weaponType = weapon;

        switch (weaponType)
        {
            case WeaponType.Hand:
                skill1 = gameObject.AddComponent<Punch>();
                skill1.Init(0.6f, SkillType.Base, 25f);
                skill1.skill_Level = 1;

                skill2 = gameObject.AddComponent<Kick>();
                skill2.Init(1f, SkillType.Base, 70f);
                skill2.skill_Level = 1;

                break;

            case WeaponType.Wood_Carving:
                skill1 = gameObject.AddComponent<Punch>();
                skill1.Init(1f, SkillType.Base, 120f);
                skill1.skill_Level = 1;

                skill2 = gameObject.AddComponent<Kick>();
                skill2.Init(1f, SkillType.Base, 70f);
                skill2.skill_Level = 1;

                break;
        }

        if (!skill5)
        {
            skill5 = gameObject.AddComponent<GetWeapon>();
            skill5.Init(1);
            skill5.skill_Level = 1;
        }
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
        if (!attackable)
        {
            fire1 = false;
            fire2 = false;
            fire3 = false;
            fire4 = false;
            fire5 = false;
            return;
        }
            

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
        // Key : J
        skill1.Cast();

        Debug.Log("스킬1 사용");
    }

    private void Skill2()
    {
        // Key : K
        skill2.Cast();

        Debug.Log("스킬2 사용");
    }

    private void Skill3()
    {
        // Key : L

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
        // Key : H

        skill5.Cast();

        Debug.Log("스킬5 사용");
    }

    public void AllCoolTimeDecline(float time)
    {
        skill1.CooltimeDecline(time);
        skill2.CooltimeDecline(time);
        skill3.CooltimeDecline(time);
        skill4.CooltimeDecline(time);
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
    public Skill GetSkillT()
    {
        return skill5;
    }
}