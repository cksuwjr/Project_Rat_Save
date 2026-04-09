using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttachedCanvas : MonoBehaviour
{
    private Entity controller;
    
    void Start()
    {
        controller = GetComponentInParent<Entity>();

        var hpPannel = transform.GetChild(0).gameObject;
        if (hpPannel.transform.GetChild(1).TryGetComponent<Image>(out var hpBar))
            controller.OnChangeHp += (prevHp, hp, maxhp) => UIManager.Instance.FillImageAnim(hpBar, prevHp, hp, maxhp);
        if (hpPannel.transform.GetChild(2).TryGetComponent<TextMeshProUGUI>(out var hpText))
            controller.OnChangeHp += (prevHp, hp, maxhp) => hpText.text = hp.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
