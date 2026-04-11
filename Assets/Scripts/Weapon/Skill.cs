using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum SkillType
{
    Base,
    Fire,
    Electronic,
}


public abstract class Skill : MonoBehaviour
{
    protected Entity controller;
    protected WeaponManager weaponManager;

    public int skill_Level;
    protected SkillType skill_Type;

    [SerializeField] protected float cooltime;
    protected float ReusableWaitTime;
    protected bool Castable;
    protected bool auto = false;
    protected Animator animator;

    public Image skillImage;
    [SerializeField] protected Image cooltimeBox;
    [SerializeField] protected TextMeshProUGUI cooltimeText;
    [SerializeField] protected Image usableUIBox;
    public string skillName;
    public string inform;


    public float damage = 1;

    [SerializeField] private AudioClip sound;
    private Color originColor;

    

    public void Init(float cooltime = 0, SkillType skillType = SkillType.Base, float damage = 0f)
    {
        if (!TryGetComponent<Animator>(out animator))
            animator = GetComponentInChildren<Animator>();

        if (!TryGetComponent<Entity>(out controller))
            Debug.LogError("해당 대상에 Entity.cs 가 없습니다!");

        TryGetComponent<WeaponManager>(out weaponManager);

        if (cooltime == 0)
            this.cooltime = 1f;

        this.damage = damage;

        Castable = true;

        if (skill_Level == 0)
            CooltimeUI(1, 0);
        else
            CooltimeUI(0, 0);

        if (cooltimeBox)
            originColor = cooltimeBox.color;

        this.skill_Type = skillType;

        if (auto)
            Cast();
    }

    public bool Cast()
    {
        if (Castable && skill_Level > 0)
        {
            StopCast(); // 기존에 실행되던 동일 스킬 중단
            StartCoroutine("Cast_");
            StartCoroutine(Cooltime());
            if (sound)
                SoundManager.Instance.PlaySound(sound);
            return true;
        }
        else
        {
            Debug.Log("해당 스킬은 재사용대기시간 중 입니다!");
            return false;
        }
    }
    public void StopCast()
    {
        StopCoroutine("Cast_");
    }
    protected abstract IEnumerator Cast_();
    protected IEnumerator Cooltime()
    {
        Castable = false;
        ReusableWaitTime = cooltime;
        CooltimeUI(1, 0);
        while (ReusableWaitTime > 0)
        {
            ReusableWaitTime -= Time.deltaTime;
            CooltimeUI(ReusableWaitTime / cooltime, ReusableWaitTime + 1);
            yield return null;
        }
        CooltimeUI(0, 0);
        ReusableWaitTime = 0;
        Castable = true;
        if (auto)
            Cast();
    }



    public void CooltimeUI(float imagefill, float textvalue)
    {
        if (cooltimeBox)
            cooltimeBox.fillAmount = imagefill;
        if (usableUIBox)
        {
            if (skill_Level > 0)
            {
                if (ReusableWaitTime <= 0.5f)
                {
                    var color = usableUIBox.color;
                    usableUIBox.fillAmount = 1;
                    color.a = ReusableWaitTime > 0.25f ? (0.5f - ReusableWaitTime) * 2 : 2 * ReusableWaitTime;
                    Debug.Log(color.a);
                    usableUIBox.color = color;
                }
            }
            if (imagefill == 0)
                usableUIBox.fillAmount = 0;
        }
        if (cooltimeText)
        {
            cooltimeText.text = ((int)textvalue).ToString();
            if (textvalue <= 0)
                cooltimeText.text = "";
        }
    }
    public void CooltimeDecline(float howmuch)
    {
        if (skill_Level < 1) return;

        ReusableWaitTime -= howmuch;
        CooltimeUI(ReusableWaitTime / cooltime, ReusableWaitTime + 1);
        if (ReusableWaitTime <= 0)
        {
            CooltimeUI(0, 0);
        }
        if (cooltimeBox)
        {
            StopCoroutine("WhiterCooltimeUI");
            cooltimeBox.color = originColor;
            StartCoroutine("WhiterCooltimeUI");
        }
    }

    private IEnumerator WhiterCooltimeUI()
    {
        float timer = 0f;

        Color color = Color.white;
        color.a = 0.6f;
        while (timer < 0.3f)
        {
            cooltimeBox.color = Color.Lerp(originColor, color, timer / 0.3f);
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0;
        while (timer < 0.3f)
        {
            cooltimeBox.color = Color.Lerp(originColor, color, 1 - (timer / 0.3f));
            timer += Time.deltaTime;
            yield return null;
        }
    }


    public virtual void Upgrade()
    {
        skill_Level += 1;

        if (skill_Level == 1)
            CooltimeUI(0, 0);
    }

    public void SetAuto(bool auto)
    {
        this.auto = auto;
    }
}