using Sirenix.OdinInspector;
using TMPro;

[InlineEditor]
public class LocalizationText : BaseLocalization
{
    public TMP_Text contentText;

    public override void RefreshContent(){
        if(id != "-1"){
            contentText.text = Localization.GetLocalizationText(id);
        }
        var fontSetting = Localization.GetFontSetting();
        contentText.font = fontSetting.font;
        contentText.material =  fontSetting.material;
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
