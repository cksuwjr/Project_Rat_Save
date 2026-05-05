using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : SingletonDestroy<UIManager>, IManager
{
    private GameObject canvas;

    private GameObject ui_Shop;
    private GameObject ui_Chat;

    public void Init()
    {
        canvas = GameObject.Find("UICanvas");
        //GameManager.Instance.Player.TryGetComponent<PlayerController>(out var player);

        ui_Chat = canvas.transform.GetChild(0).gameObject;

        var chatPanel = ui_Chat.transform.GetChild(0).gameObject;
        var openShopBtn = chatPanel.transform.GetChild(3).GetComponent<Button>();
        openShopBtn.onClick.AddListener(UI_Shop_OpenClose);

        var nextBtn = chatPanel.transform.GetChild(4).GetComponent<Button>();
        nextBtn.onClick.AddListener(() => { UI_Chat_OpenClose(); GameManager.Instance.ReadyToStage(); });

        /////////////////////////

        ui_Shop = canvas.transform.GetChild(1).gameObject;

        var viewPort = ui_Shop.transform.GetChild(0).GetChild(1).GetComponent<ScrollRect>().viewport;
        var content = viewPort.GetChild(0);
        var slot = content.GetChild(0).gameObject;

        var slot1 = Instantiate(slot, content);
        SetSlot(slot1, "공격력 증가");

        var slot2 = Instantiate(slot, content);
        SetSlot(slot2, "공격속도\n증가");

        var slot3 = Instantiate(slot, content);
        SetSlot(slot3, "체력 증가");

        var slot4 = Instantiate(slot, content);
        SetSlot(slot4, "아무거나 1");

        var slot5 = Instantiate(slot, content);
        SetSlot(slot5, "아무거나 2");


        var exitShopBtn = ui_Shop.transform.GetChild(0).GetChild(2).GetComponent<Button>();
        exitShopBtn.onClick.AddListener(() => { ui_Shop.SetActive(false); ui_Chat.SetActive(true); });

    }

    public void SetSlot(GameObject slot, string itemText = "", Sprite itemSprite = null, UnityAction btnEvent = null)
    {
        slot.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = itemText;
        if(itemSprite != null) slot.transform.GetChild(1).GetComponent<Image>().sprite = itemSprite;
        if (btnEvent != null) slot.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(btnEvent);
        else slot.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();

        slot.SetActive(true);
    }

    public void UI_Shop_OpenClose()
    {
        if (ui_Chat.activeSelf) UI_Chat_OpenClose();

        ui_Shop.SetActive(!ui_Shop.activeSelf);

    }

    public void UI_Chat_OpenClose()
    {
        ui_Chat.SetActive(!ui_Chat.activeSelf);
    }

    

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

        bar.transform.parent.parent.gameObject.SetActive(fillValue > 0);

        
    }

}