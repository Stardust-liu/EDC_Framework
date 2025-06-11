using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class FontSetting{
    public TMP_FontAsset font;
    public Dictionary<FontMaterialType, Material> fontMaterial;
}

[CreateAssetMenu(fileName = "LocalizationSetting", menuName = "创建.Assets文件/FrameworkTool/LocalizationSetting")]
public class LocalizationSetting : SerializedScriptableObject
{
    [DictionaryDrawerSettings(KeyLabel ="支持的语言", ValueLabel ="字体信息")]
    public Dictionary<SystemLanguage, FontSetting> LanguageSupport;

    /// <summary>
    /// 获取字体设置
    /// </summary>
    public FontSetting GetFontSetting(SystemLanguage language){
        return LanguageSupport[language];
    }

    /// <summary>
    /// 检查是否支持该语言
    /// </summary>
    public bool CheckIsSupport(SystemLanguage language){
        return LanguageSupport.ContainsKey(language);
    }
}
