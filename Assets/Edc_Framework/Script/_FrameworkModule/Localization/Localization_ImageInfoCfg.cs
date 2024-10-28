using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization_ImageInfoCfg : BaseLocalizationInfoCfg<Localization_ImageInfoCfg>
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
    /// 获取本地化图像路径
    /// </summary>
    public string GetLocalizationImage(LanguageId currentLanguage, string key){
        return localizationInfo[currentLanguage][key];
    }
}
