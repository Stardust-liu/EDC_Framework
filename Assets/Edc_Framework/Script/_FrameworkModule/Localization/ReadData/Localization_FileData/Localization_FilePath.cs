using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization_FileDataVO
{
    public ResourcePath resourcePath;
    public LocalizationType localizationType;

    public Localization_FileDataVO(ResourcePath _resourcePath, LocalizationType _localizationType)
    {
        resourcePath = _resourcePath;
        localizationType = _localizationType;
    }
}

public class Localization_FileData : ParsCsv<Localization_FileData>
{
    private Dictionary<SystemLanguage, List<Localization_FileDataVO>> localizationInfo;

    protected override void InitData()
    {
        if (localizationInfo == null)
        {
            localizationInfo = new(RowCount);
        }
        else
        {
            foreach (var item in localizationInfo)
            {
                item.Value.Clear();
            }
        }
    }

    protected override void SetData()
    {
        var resourcePath = new ResourcePath(GetString("AB_FileNmae"), GetString("AssetsPath"));
        var localizationType = GetEnum<LocalizationType>("LocalizationType");
        var language = GetEnum<SystemLanguage>("SystemLanguage");
        var Localization_FileDataVO = new Localization_FileDataVO(resourcePath, localizationType);
        if (!localizationInfo.ContainsKey(language))
        {
            localizationInfo.Add(language, new List<Localization_FileDataVO>() { Localization_FileDataVO });
        }
        else
        {
            localizationInfo[language].Add(Localization_FileDataVO);
        }
    }

    /// <summary>
    /// 获取本地化文件信息
    /// </summary>
    public List<Localization_FileDataVO> GetLocalizationFileData(SystemLanguage systemLanguage)
    {
        return localizationInfo[systemLanguage];
    }
}
