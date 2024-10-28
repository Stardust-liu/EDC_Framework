using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization_AudioInfoCfg : BaseLocalizationInfoCfg<Localization_AudioInfoCfg>
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
    /// 获取本地化音频路径
    /// </summary>
    public string GetLocalizationAudio(LanguageId currentLanguage, string key){
        return localizationInfo[currentLanguage][key];
    }
}
