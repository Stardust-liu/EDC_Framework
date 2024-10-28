using UnityEngine;

public class LocalizationAudio : BaseLocalization
{
    public AudioType audioType;
    private static AudioController audioManager = Hub.Audio;
    private AudioClip audioClip;

    public override void RefreshContent(){
        if(id != "-1"){
            audioClip = Localization.GetLocalizationAudio(id, audioType);
        }
    }

    public override void RefreshContent(string _id, bool isOverrideID = false){
        base.RefreshContent(_id, isOverrideID);
        audioClip = Localization.GetLocalizationAudio(_id, audioType);
    }

    public void Play(){
        switch (audioType)
        {
            case AudioType.SoundBg:
                    audioManager.PlaysoundBg(audioClip);
                break;
            case AudioType.SoundEffect:
                    audioManager.PlaysoundBg(audioClip);
                break;
            case AudioType.SoundDialogue:
                    audioManager.PlaySoundDialogue(audioClip);
                break;
        }
    }
}
