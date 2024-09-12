using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LocalizationInfoCfg : ParsCsv<LocalizationInfoCfg>
{
    public static Dictionary<LanguageId , Dictionary<string, string>> localizationInfo;
    
    private static LanguageId[] _languageId;
    private static LanguageId[] LanguageId{
        get
        { 
            _languageId ??= (LanguageId[])Enum.GetValues(typeof(LanguageId));
            return _languageId;
        }
        set{
           _languageId = value;
        }
    }
    private static readonly int count = Enum.GetValues(typeof(LanguageId)).Length;
    private bool isAdd;

    public void Init(Dictionary<LanguageId , Dictionary<string, string>> keyValuePairs){
        localizationInfo = keyValuePairs;
    }

    public void AddLocalizationAsset(TextAsset csv){
        isAdd = true;
        ParseData(csv);
    }
    public void RemoveLocalizationAsset(TextAsset csv){
        isAdd = false;
        ParseData(csv);
    }

    protected override void SetData()
    {
        if(isAdd){
            for (var i = 0; i < count; i++)
            {
                var id = LanguageId[i];
                localizationInfo[id].Add(GetString("ID"), GetString(id.ToString()));
            }
        }
        else{
            for (var i = 0; i < count; i++)
            {
                var id = LanguageId[i];
                localizationInfo[id].Remove(GetString("ID"));
            }
        }
    }
}
