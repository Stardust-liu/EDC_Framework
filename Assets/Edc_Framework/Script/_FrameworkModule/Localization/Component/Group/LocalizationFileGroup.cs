using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class LocalizationFileItem{
    [LabelText("文件对应的本地化类型")]
    public LocalizationType localizationType;
    [LabelText("本地化文件")]
    public TextAsset localizationCsvFile;
}

public class LocalizationFileGroup : MonoBehaviour
{
    [LabelText("本地化文件列表")]
    public LocalizationFileItem[] localizationFileItems;
    private static LocalizationManager localization;
    protected static LocalizationManager Localization{
        get{
            localization ??= Hub.Localization;
            return localization;
        }
    }

    private void OnEnable() {
        if(FrameworkManager.isInitFinish){
            AddLocalizationData();
        }
    }

    private void OnDisable()
    {   
        if(FrameworkManager.isInitFinish){
            RemoveLocalizationData();
        }
    }

    /// <summary>
    /// 添加数据
    /// </summary>
    private void AddLocalizationData(){
        foreach (var item in localizationFileItems)
        {
            AddLocalizationData(item.localizationType, item.localizationCsvFile);
        }
    }

    /// <summary>
    /// 移除数据
    /// </summary>
    private void RemoveLocalizationData(){
        foreach (var item in localizationFileItems)
        {
            RemoveLocalizationData(item.localizationType, item.localizationCsvFile);
        }
    }

    private void AddLocalizationData(LocalizationType localizationType, TextAsset localizationCsvFile){
        switch (localizationType)
        {
            case LocalizationType.Text:
                    Localization.AddTextAsset(localizationCsvFile);
                break;
            case LocalizationType.Size:
                    Localization.AddSizeAsset(localizationCsvFile);
                break;
            case LocalizationType.Image:
                    Localization.AddImageAsset(localizationCsvFile);
                break;
            case LocalizationType.Audio:
                    Localization.AddAudioAsset(localizationCsvFile);
                break;
        }
    }

    private void RemoveLocalizationData(LocalizationType localizationType, TextAsset localizationCsvFile){
        switch (localizationType)
        {
            case LocalizationType.Text:
                    Localization.RemoveTextAsset(localizationCsvFile);
                break;
            case LocalizationType.Size:
                    Localization.RemoveSizeAsset(localizationCsvFile);
                break;
            case LocalizationType.Image:
                    Localization.RemoveImageAsset(localizationCsvFile);
                break;
            case LocalizationType.Audio:
                    Localization.RemoveAudioAsset(localizationCsvFile);
                break;
        }
    }
}
