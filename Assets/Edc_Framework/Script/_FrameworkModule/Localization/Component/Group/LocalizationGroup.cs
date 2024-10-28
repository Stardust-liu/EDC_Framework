using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
public enum LocalizationType{
    Text,
    Size,
    Image,
    Audio,
}

public class LocalizationGroup : MonoBehaviour
{
    [LabelText("本地化Item列表")]
    public BaseLocalization[] localizationItem;
    private bool isAddDataFinish;
    private LanguageId showLanguage = LanguageId.zh_Hans;
    private static LocalizationManager localization;
    protected static LocalizationManager Localization{
        get{
            localization ??= Hub.Localization;
            return localization;
        }
    }
   
    private void OnEnable() {
        if(FrameworkManager.isInitFinish){
            if(isAddDataFinish){
                SetLanguage(Localization.CurrentLanguage);
            }
            LocalizationManager.eventCenter.AddListener<LanguageId>(LocalizationEventName.changeLanguage, SetLanguage);
        }
    }

    private void Start()
    {
        SetLanguage(Localization.CurrentLanguage);
        isAddDataFinish = true;
    }

    private void OnDisable()
    {   
        if(FrameworkManager.isInitFinish){
            LocalizationManager.eventCenter.RemoveListener<LanguageId>(LocalizationEventName.changeLanguage, SetLanguage);
        }
    }


    private void SetLanguage(LanguageId languageId){
        if(showLanguage != languageId){
            foreach (var item in localizationItem)
            {
                item.RefreshContent();
            }
            showLanguage = languageId;
        }
    }
}
