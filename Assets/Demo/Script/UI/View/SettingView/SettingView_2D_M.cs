using System;
using System.Collections;
using UnityEngine;

public class SettingView_2D_M : BaseUI_Model
{
    protected override void Init()
    {
        RegisterData<LocalizationManager>(Hub.Localization);
        RegisterData<AudioManager>(Hub.Audio);
    }

    /// <summary>
    /// 更新语言
    /// </summary>
    public void ChangeLanguage(int index)
    {
        Get<LocalizationManager>().ChangeLanguage(index);
    }

    /// <summary>
    /// 获取当前语言索引
    /// </summary>
    public int GetLanguageIndex()
    {
        return Get<LocalizationManager>().GetCurrentLanguageIndex();
    }

    /// <summary>
    /// 获取语言文字信息
    /// </summary>
    public string[] GetLanguageInfo()
    {
        return Get<LocalizationManager>().GetLanguageInfo();
    }

    /// <summary>
    /// 设置背景音音量
    /// </summary>
    public void SetSoundBgVolume(float volume)
    {
        Get<AudioManager>().SetSoundBgVolume(volume);
    }

    /// <summary>
    /// 获取背景音音量
    /// </summary>
    public float GetsoundBgVolume()
    {
        return Get<AudioManager>().SoundBgVolume;
    }

    /// <summary>
    /// 设置音效音量
    /// </summary>
    public void SetSoundEffectVolume(float volume)
    {
        Get<AudioManager>().SetSoundEffectVolume(volume);
    }
    
    /// <summary>
    /// 获取音效音量
    /// </summary>
    public float GetSoundEffectVolume(){
        return Get<AudioManager>().SoundEffectVolume;
    }
}
