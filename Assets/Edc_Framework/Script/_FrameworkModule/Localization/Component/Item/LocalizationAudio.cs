using UnityEngine;
using Sirenix.OdinInspector;
[InlineEditor]
public class LocalizationAudio : BaseLocalization
{
    public AudioType audioType;
    private static AudioManager audioManager = Hub.Audio;
    private AudioClip audioClip;
    private string abName;

    private void Start()
    {
        SetABName();
    }

    public override void RefreshContent()
    {
        if (id != "-1")
        {
            audioClip = Localization.GetLocalizationAsset<AudioClip>(abName, id);
        }
    }

    public override void RefreshContent(string _id, bool isOverrideID = false)
    {
        base.RefreshContent(_id, isOverrideID);
        audioClip = Localization.GetLocalizationAsset<AudioClip>(abName, _id);
    }

    private void SetABName()
    {
        switch (audioType)
        {
            case AudioType.SoundBg:
                abName = "soundBg";
                break;
            case AudioType.SoundEffect:
                abName = "soundEffect";
                break;
            case AudioType.SoundDialogue:
                abName = "soundDialogue";
                break;
        }
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
