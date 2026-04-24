using UnityEngine;
using Sirenix.OdinInspector;
[InlineEditor]
public class LocalizationAudio : BaseLocalization
{
    public AudioType audioType;
    private static AudioManager audioManager = Hub.Audio;
    private AudioClip audioClip;

    public override void RefreshContent()
    {
        if (!string.IsNullOrEmpty(id) && id != "-1")
        {
            audioClip = Localization.GetLocalizationAsset<AudioClip>(id);
        }
    }

    public override void RefreshContent(string _id, bool isOverrideID = false)
    {
        base.RefreshContent(_id, isOverrideID);
        audioClip = Localization.GetLocalizationAsset<AudioClip>(_id);
    }

    public void Play()
    {
        switch (audioType)
        {
            case AudioType.SoundBg:
                audioManager.PlaySoundBg(audioClip);
                break;
            case AudioType.SoundEffect:
                audioManager.PlaySoundBg(audioClip);
                break;
            case AudioType.SoundDialogue:
                audioManager.PlaySoundDialogue(audioClip);
                break;
        }
    }
}
