using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationExample : MonoBehaviour
{
    public Button set_zh_Hans;
    public Button set_zh_Hant;
    public Button set_en;
    public Button set_ja;
    public Button set_ko;

    public RectTransform mark;
    private LocalizationManager localization;

    void Start()
    {
        localization = Hub.Localization;
        set_zh_Hans.onClick.AddListener(ClickChange_zh_Hans);
        set_zh_Hant.onClick.AddListener(ClickChange_zh_Hant);
        set_en.onClick.AddListener(ClickChange_en);
        set_ja.onClick.AddListener(ClickChange_ja);
        set_ko.onClick.AddListener(ClickChange_ko);
    }

    private void ClickChange_zh_Hans(){
        localization.ChangeLange(LanguageId.zh_Hans);
        SetMarkParent(set_zh_Hans);
    }
    private void ClickChange_zh_Hant(){
        localization.ChangeLange(LanguageId.zh_Hant);
        SetMarkParent(set_zh_Hant);
    }
    private void ClickChange_en(){
        localization.ChangeLange(LanguageId.en);
        SetMarkParent(set_en);
    }
    private void ClickChange_ja(){
        localization.ChangeLange(LanguageId.ja);
        SetMarkParent(set_ja);
    }
    private void ClickChange_ko(){
        localization.ChangeLange(LanguageId.ko);
        SetMarkParent(set_ko);
    }

    private void SetMarkParent(Button btn){
        mark.SetParent(btn.GetComponent<RectTransform>());
        mark.anchoredPosition = Vector2.one;
    }
}
