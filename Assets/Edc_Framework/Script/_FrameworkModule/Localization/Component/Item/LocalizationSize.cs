using UnityEngine;

public class LocalizationSize : BaseLocalization
{
    public RectTransform rectTransform;

    public override void RefreshContent(){
        if(id != "-1"){
            rectTransform.sizeDelta = Localization.GetLocalizationSize(id);
        }
    }

    public override void RefreshContent(string _id, bool isOverrideID = false){
        base.RefreshContent(_id, isOverrideID);
        rectTransform.sizeDelta = Localization.GetLocalizationSize(_id);
    }
}
