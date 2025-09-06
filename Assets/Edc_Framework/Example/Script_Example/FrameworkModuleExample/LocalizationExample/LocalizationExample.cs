using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationExample : MonoBehaviour
{
    public Button set_zh_Hans;
    public Button set_en;
    public RectTransform mark;
    private LocalizationManager localization;

    void Start()
    {
        localization = Hub.Localization;
        set_zh_Hans.onClick.AddListener(ClickChange_zh_Hans);
        set_en.onClick.AddListener(ClickChange_en);
    }

    private void ClickChange_zh_Hans()
    {
        localization.ChangeLanguage(SystemLanguage.ChineseSimplified);
        SetMarkParent(set_zh_Hans);
    }
    private void ClickChange_en(){
        localization.ChangeLanguage(SystemLanguage.English);
        SetMarkParent(set_en);
    }

    private void SetMarkParent(Button btn){
        mark.SetParent(btn.GetComponent<RectTransform>());
        mark.anchoredPosition = Vector2.one;
    }
}
