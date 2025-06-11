using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[InlineEditor]
public class LocalizationImage : BaseLocalization
{
    public Image contentImage;
    public override void RefreshContent()
    {
        if (id != "-1")
        {
            contentImage.sprite = Localization.GetLocalizationAsset<Sprite>(id, "sprite");
        }
    }

    public override void RefreshContent(string _id, bool isOverrideID = false)
    {
        base.RefreshContent(_id, isOverrideID);
        contentImage.sprite = Localization.GetLocalizationAsset<Sprite>(_id, "sprite");
    }
}
