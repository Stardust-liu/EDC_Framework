using Sirenix.OdinInspector;
using TMPro;

public enum FontMaterialType{
    UI,
    UI_BlackOutline,
    Scene,
    Scene_BlackOutline,
}

[InlineEditor]
public class LocalizationText : BaseLocalization
{
    public FontMaterialType fontMaterialType;
    public TMP_Text contentText;

    public override void RefreshContent(){
        if(!string.IsNullOrEmpty(id)){
            contentText.text = Localization.GetLocalizationText(id);
        }
        var fontSetting = Localization.GetFontSetting();
        contentText.font = fontSetting.font;
        contentText.fontSharedMaterial = fontSetting.fontMaterial[fontMaterialType];
    }
    
    public override void RefreshContent(string _id, bool isOverrideID = false){
        base.RefreshContent(_id, isOverrideID);
        contentText.text = Localization.GetLocalizationText(_id);
    }

    /// <summary>
    /// 清空文字内容
    /// </summary>
    public void CleanTextContent(){
        contentText.text = null;
    }
}
