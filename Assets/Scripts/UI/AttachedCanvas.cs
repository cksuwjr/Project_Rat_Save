
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttachedCanvas : MonoBehaviour
{
    private Entity controller;
    bool isInited = false;

    private void Awake()
    {
        controller = GetComponentInParent<Entity>();

        controller.OnInit += InitUI;
    }

    void InitUI()
    {
        if (isInited) return;
        else isInited = true;

        var hpPannel = transform.GetChild(0).gameObject;
        if (hpPannel.transform.GetChild(1).TryGetComponent<Image>(out var hpBar))
            controller.OnChangeHp += (prevHp, hp, maxhp) => UIManager.Instance.FillImageAnim(hpBar, prevHp, hp, maxhp);
        if (hpPannel.transform.GetChild(2).TryGetComponent<TextMeshProUGUI>(out var hpText))
            controller.OnChangeHp += (prevHp, hp, maxhp) => hpText.text = $"{hp:F0}";


        var gagePannel = transform.GetChild(1).gameObject;
        if (gagePannel.transform.GetChild(1).TryGetComponent<Image>(out var gageBar))
            controller.OnChangeGage += (prevGage, gage, maxGage) => UIManager.Instance.FillImageAnim(gageBar, prevGage, gage, maxGage);
        //if (gagePannel.transform.GetChild(2).TryGetComponent<TextMeshProUGUI>(out var gageText))
        //    controller.OnChangeGage += (prevGage, gage, maxGage) => gageText.text = $"{gage:F0}";
    }
}
