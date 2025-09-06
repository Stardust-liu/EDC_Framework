using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingView_2D : BaseReturnableView<SettingView_2D_M>
{
    public OptionItemToggle language;
    public OptionItemSlider sfx;
    public OptionItemSlider bgm;

    protected override void Init()
    {
        base.Init();
        language.SetInitAction(GetLanguageToggleInfo(), ChangeOptionItem, ExitOptionItem);
        sfx.SetInitAction(GetSFXInfo(), ChangeOptionItem, ExitOptionItem);
        bgm.SetInitAction(GetBGMInfo(), ChangeOptionItem, ExitOptionItem);
    }

    private void SetLanguage(int index)
    {
        model.ChangeLanguage(index);
    }

    private void SetSFX(float value)
    {
        model.SetSoundEffectVolume(value);
    }

    private void SetBGM(float value)
    {
        model.SetSoundBgVolume(value);
    }

    private OptionItemToggleInfo GetLanguageToggleInfo()
    {
        return new OptionItemToggleInfo(SetLanguage, model.GetLanguageInfo(), model.GetLanguageIndex());
    }
    private OptionItemSliderInfo GetSFXInfo()
    {
        return new OptionItemSliderInfo(SetSFX, model.GetSoundEffectVolume());
    }
    private OptionItemSliderInfo GetBGMInfo()
    {
        return new OptionItemSliderInfo(SetBGM, model.GetsoundBgVolume());
    }
}
