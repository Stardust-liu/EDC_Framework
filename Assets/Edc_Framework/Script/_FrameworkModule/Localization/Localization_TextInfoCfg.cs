using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Localization_TextInfoCfg : BaseLocalizationInfoCfg<Localization_TextInfoCfg>
{
    private static Dictionary<LanguageId , Dictionary<string, string>> localizationInfo = new();

    public void Init(LanguageId[] languageId, int count){
        Init(count);
        for (var i = 0; i < count; i++)
        {
            localizationInfo.Add(languageId[i], new Dictionary<string, string>());
        }
    }

    protected override void SetData()
    {
        var LanguageId = Hub.Localization.LanguageId;
        if(isAddInfo){
            for (var i = 0; i < supportLanguageCount; i++)
            {
                var id = LanguageId[i];
                localizationInfo[id].Add(GetString("ID"), GetString(id.ToString()));
            }
        }
        else{
            for (var i = 0; i < supportLanguageCount; i++)
            {
                var id = LanguageId[i];
                localizationInfo[id].Remove(GetString("ID"));
            }
        }
    }

    /// <summary>
    /// 获取本地化文字
    /// </summary>
    public string GetLocalizationText(LanguageId currentLanguage, string key){
        return localizationInfo[currentLanguage][key];
    }
}
