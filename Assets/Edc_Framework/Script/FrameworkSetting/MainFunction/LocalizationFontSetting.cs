using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class FontSetting{
    public TMP_FontAsset font;
    public Material material;
}

[CreateAssetMenu(fileName = "LocalizationFontSetting", menuName = "创建.Assets文件/LocalizationFontSetting")]
public class LocalizationFontSetting : SerializedScriptableObject
{
   public Dictionary<LanguageId, FontSetting> fontSetting;
}
