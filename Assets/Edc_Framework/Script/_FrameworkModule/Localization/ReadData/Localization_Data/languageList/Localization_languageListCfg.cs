using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization_languageListCfg : ParsCsv<Localization_languageListCfg>
{
    public string[] localizationInfo;
    public SystemLanguage[] languageOrder;

    protected override void InitData()
    {
        localizationInfo = new string[RowCount];
        languageOrder = new SystemLanguage[RowCount];
    }

    protected override void SetData()
    {
        languageOrder[CurrentReadIndex] = GetEnum<SystemLanguage>("Language");
        localizationInfo[CurrentReadIndex] = GetString("Desc");
    }
}
