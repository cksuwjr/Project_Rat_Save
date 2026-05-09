using UnityEngine;

public enum WeaponType
{
    Hand,
    Wood_Carving,
    Bow,
}

public class WeaponManager : MonoBehaviour
{
    private WeaponType weaponType = 0;
    public Transform hand;
    public float weaponRange;

    private bool[] fire = new bool[5] { false, false, false, false, false };

    [SerializeField] private Skill[] skills = new Skill[5] {null, null, null, null, null};

    public bool attackable = true;

    public Skill[] Skills { get { return skills; } }

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
        if (skills[0]) { Destroy(skills[0]); skills[0] = null; }
        if (skills[1]) { Destroy(skills[1]); skills[1] = null; }

        weaponType = weapon;

        switch (weaponType)
        {
            case WeaponType.Hand:
                skills[0] = gameObject.AddComponent<Punch>();
                skills[0].Init(KeyInput.Fire1, 0.6f, SkillType.Base, 25f);
                skills[0].skill_Level = 1;

                skills[1] = gameObject.AddComponent<Kick>();
                skills[1].Init(KeyInput.Fire2, 0.6f, SkillType.Base, 70f);
                skills[1].skill_Level = 1;

                weaponRange = 2f;

                break;

            case WeaponType.Wood_Carving:
                skills[0] = gameObject.AddComponent<Punch>();
                skills[0].Init(KeyInput.Fire1, 0.7f, SkillType.Base, 120f);
                skills[0].skill_Level = 1;

                skills[1] = gameObject.AddComponent<Kick>();
                skills[1].Init(KeyInput.Fire2, 1f, SkillType.Base, 70f);
                skills[1].skill_Level = 1;

                weaponRange = 2f;

                break;

            case WeaponType.Bow:
                skills[0] = gameObject.AddComponent<ShootArrow>();
                skills[0].Init(KeyInput.Fire1, 0.2f, SkillType.Base, 35f);
                skills[0].skill_Level = 1;

                skills[1] = gameObject.AddComponent<RollingShoot>();
                skills[1].Init(KeyInput.Fire2, 1f, SkillType.Base, 35f);
                skills[1].skill_Level = 1;

                weaponRange = 8f;

                break;
        }

        if (!skills[2])
        {
            skills[2] = gameObject.AddComponent<RollingRat>();
            skills[2].Init(KeyInput.Fire3, 1);
            skills[2].skill_Level = 1;
        }

        if (!skills[4])
        {
            skills[4] = gameObject.AddComponent<GetWeapon>();
            skills[4].Init(KeyInput.Fire5, 1);
            skills[4].skill_Level = 1;
        }

        //skill1 = gameObject.AddComponent<Punch>();
        //skill1.Init(KeyInput.Fire1, 0.6f, SkillType.Base, 25f);
        //skill1.skill_Level = 1;

        //skill2 = gameObject.AddComponent<Kick>();
        //skill2.Init(KeyInput.Fire2, 1f, SkillType.Base, 70f);
        //skill2.skill_Level = 1;
    }


    public void Fire(KeyInput input)
    {
        switch (input)
        {
            case KeyInput.Fire1: fire[0] = true; break;
            case KeyInput.Fire2: fire[1] = true; break;
            case KeyInput.Fire3: fire[2] = true; break;
            case KeyInput.Fire4: fire[3] = true; break;
            case KeyInput.Fire5: fire[4] = true; break;
        }
    }

    private void FixedUpdate()
    {
        if (!attackable)
        {
            fire[0] = false;
            fire[1] = false;
            fire[2] = false;
            fire[3] = false;
            fire[4] = false;
            return;
        }
            

        for(int i = 0; i < fire.Length; i++)
        {
            if (fire[i])
            {
                skills[i]?.Cast();
                fire[i] = false;
            }
        }
    }

    public void AllCoolTimeDecline(float time)
    {
        for (int i = 0; i < skills.Length; i++)
            skills[i].CooltimeDecline(time);
    }
}