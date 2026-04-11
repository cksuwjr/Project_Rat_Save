using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : SingletonDestroy<UIManager>, IManager
{
    private GameObject canvas;
    private GameObject playerCanvas;

    public void Init()
    {
        canvas = GameObject.Find("Canvas");
        GameManager.Instance.Player.TryGetComponent<PlayerController>(out var player);
    }

/*
    public void SkillSelect()
    {
        TimeManager.Instance.Stop();

        var wm = GameManager.Instance.Player.GetComponent<WeaponManager>();

        for (int i = 0; i < selectPannel.transform.childCount; i++)
            selectPannel.transform.GetChild(i).gameObject.SetActive(false);


        SkillCard card;
        GameObject g;
        Skill skill1;
        Skill skill2;
        Skill skill3;
        Skill skill4;
        Skill skill5;
        Button btn;

        skill1 = wm.GetSkillBasic();
        if (skill1.skill_Level < 5)
        {
            g = selectPannel.transform.GetChild(0).gameObject;
            g.SetActive(true);
            if (g.TryGetComponent<SkillCard>(out card))
                card.Init(skill1.skillImage.sprite, skill1.skillName, skill1.inform);

            if (g.TryGetComponent<Button>(out btn))
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    skill1.Upgrade();
                    TimeManager.Instance.Resume();
                    CloseSelectPannel();
                }
                );
            }

        }

        skill2 = wm.GetSkillQ();
        if (skill2.skill_Level < 5)
        {
            g = selectPannel.transform.GetChild(1).gameObject;
            g.SetActive(true);
            if (g.TryGetComponent<SkillCard>(out card))
                card.Init(skill2.skillImage.sprite, skill2.skillName, skill2.inform);
            if (g.TryGetComponent<Button>(out btn))
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    skill2.Upgrade();
                    TimeManager.Instance.Resume();
                    CloseSelectPannel();
                }
                );
            }
        }

        skill3 = wm.GetSkillW();
        if (skill3.skill_Level < 5)
        {
            g = selectPannel.transform.GetChild(2).gameObject;
            g.SetActive(true);
            if (g.TryGetComponent<SkillCard>(out card))
                card.Init(skill3.skillImage.sprite, skill3.skillName, skill3.inform);
            if (g.TryGetComponent<Button>(out btn))
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    skill3.Upgrade();
                    TimeManager.Instance.Resume();
                    CloseSelectPannel();
                }
                );
            }
        }

        skill4 = wm.GetSkillE();
        if (skill4.skill_Level < 5)
        {
            g = selectPannel.transform.GetChild(3).gameObject;
            g.SetActive(true);
            if (g.TryGetComponent<SkillCard>(out card))
                card.Init(skill4.skillImage.sprite, skill4.skillName, skill4.inform);
            if (g.TryGetComponent<Button>(out btn))
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    skill4.Upgrade();
                    TimeManager.Instance.Resume();
                    CloseSelectPannel();
                }
                );
            }
        }

        skill5 = wm.GetSkillR();
        if (skill5.skill_Level < 5)
        {
            g = selectPannel.transform.GetChild(4).gameObject;
            g.SetActive(true);
            if (g.TryGetComponent<SkillCard>(out card))
                card.Init(skill5.skillImage.sprite, skill5.skillName, skill5.inform);
            if (g.TryGetComponent<Button>(out btn))
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() =>
                {
                    skill5.Upgrade();
                    TimeManager.Instance.Resume();
                    CloseSelectPannel();
                }
                );
            }
        }

        if (wm.GetSkillBasic().skill_Level < 5)
            selectPannel.transform.GetChild(0).gameObject.SetActive(true);

        if (wm.GetSkillQ().skill_Level < 5)
            selectPannel.transform.GetChild(1).gameObject.SetActive(true);

        if (wm.GetSkillW().skill_Level < 5)
            selectPannel.transform.GetChild(2).gameObject.SetActive(true);

        if (wm.GetSkillE().skill_Level < 5)
            selectPannel.transform.GetChild(3).gameObject.SetActive(true);

        if (wm.GetSkillR().skill_Level < 5)
            selectPannel.transform.GetChild(4).gameObject.SetActive(true);

        selectPannel.SetActive(true);
    }
*/

    public void FillImageAnim(Image bar, float nowValue, float fillValue, float maxValue)
    {
        StartCoroutine(FillBar(bar, nowValue, fillValue, maxValue));
    }

    private IEnumerator FillBar(Image bar, float nowValue, float fillValue, float maxValue)
    {
        float time = 0.5f;
        float timer = 0f;
        bar.fillAmount = nowValue / maxValue;
        while (timer <= time)
        {
            bar.fillAmount = Mathf.Lerp(nowValue, fillValue, timer / time) / maxValue;
            yield return null;
            timer += Time.deltaTime;
        }
        bar.fillAmount = fillValue / maxValue;

        if (fillValue <= 0)
            bar.transform.parent.parent.gameObject.SetActive(fillValue > 0);
        
    }

}