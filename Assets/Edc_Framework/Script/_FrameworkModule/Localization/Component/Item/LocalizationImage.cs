using UnityEngine;
using UnityEngine.UI;

public class LocalizationImage : BaseLocalization
{
    public Image contentImage;
    public override void RefreshContent(){
        if(id != "-1"){
            contentImage.sprite = Localization.GetLocalizationImage(id);
        }
    }

    public override void RefreshContent(string _id, bool isOverrideID = false){
        base.RefreshContent(_id, isOverrideID);
        contentImage.sprite = Localization.GetLocalizationImage(_id);
    }
}
