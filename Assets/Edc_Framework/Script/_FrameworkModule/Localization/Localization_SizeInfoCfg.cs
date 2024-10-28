using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization_SizeInfoCfg : BaseLocalizationInfoCfg<Localization_SizeInfoCfg>
{
    private static Dictionary<LanguageId , Dictionary<string, Vector2>> localizationInfo = new();

    public void Init(LanguageId[] languageId, int count){
        Init(count);
        for (var i = 0; i < count; i++)
        {
            localizationInfo.Add(languageId[i], new Dictionary<string, Vector2>());
        }
    }

    protected override void SetData()
    {
        var LanguageId = Hub.Localization.LanguageId;
        if(isAddInfo){
            for (var i = 0; i < supportLanguageCount; i++)
            {
                var id = LanguageId[i];
                var data = GetStringArray(id.ToString());
                var pos = new Vector2(int.Parse(data[0]) , int.Parse(data[1]));
                localizationInfo[id].Add(GetString("ID"), pos);
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
    /// 获取本地化尺寸
    /// </summary>
    public Vector2 GetLocalizationSize(LanguageId currentLanguage, string key){
        return localizationInfo[currentLanguage][key];
    }
}
