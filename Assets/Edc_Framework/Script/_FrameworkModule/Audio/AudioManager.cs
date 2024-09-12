using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ArchiveData;

public class AudioManager
{
    private AudioSource bGM1;
    private AudioSource bGM2;
    private static SoundBgSetting soundBg;
    private static SoundEffectSetting soundEffect;
    private static SoundDialogueSetting soundDialogue;
    public float SoundMainVolume{get; private set;}
    public float SoundBgVolume{get; private set;}
    public float SoundEffectVolume{get; private set;}
    public float SoundDialogueVolume{get; private set;}

    public float SoundBgOffsetVolume {get{return SoundBgVolume * SoundMainVolume;}}
    public float SoundEffectOffsetVolume {get{return SoundEffectVolume * SoundMainVolume;}}
    public float SoundDialogueOffsetVolume {get{return SoundDialogueVolume * SoundMainVolume;}}

    private readonly AudioData audioData;

    public AudioManager(){
        var framework = Hub.Framework;
        audioData = GameArchive.AudioData;
        bGM1 = framework.bGM1;
        bGM2 = framework.bGM2;
        soundBg = FrameworkManager.SoundBgSetting;
        soundEffect = FrameworkManager.SoundEffectSetting;
        soundDialogue = FrameworkManager.SoundDialogueSetting;
        SFXPool.InitPool(BasePool.GetFrameworkPool("SFX"), framework.sFXParent, 5, true);
        VOPool.InitPool(BasePool.GetFrameworkPool("VO"), framework.vOParent, 5, true);
        SetInitVolumeValue();
    }

    private void SetInitVolumeValue(){
        SoundMainVolume = audioData.soundMainVolume;
        SoundBgVolume = audioData.soundMainVolume;
        SoundEffectVolume = audioData.soundMainVolume;
        SoundDialogueVolume = audioData.soundMainVolume;
    }
#region 主音音量相关
    /// <summary>
    /// 设置主音量
    /// </summary>
    public void SetSoundMainVolume(float volume){
        SoundMainVolume = volume;
        bGM1.volume = bGM2.volume = SoundBgOffsetVolume;
        SFXPool.SetAllSoundEffectVolume(SoundEffectOffsetVolume);
        VOPool.SetAllSoundEffectVolume(SoundDialogueOffsetVolume);
        audioData.UpdtaeSoundMainVolume(volume);
    }
#endregion
#region 背景音相关
    /// <summary>
    /// 设置背景音乐音量
    /// </summary>
    public void SetsoundBgVolume(float volume){
        SoundBgVolume = volume;
        bGM1.volume = SoundBgOffsetVolume;
        bGM2.volume = SoundBgOffsetVolume;
        audioData.UpdateSoundBgVolume(volume);
    }

    /// <summary>
    /// 播放背景音
    /// </summary>
    public void PlaysoundBg(string audioName, string sceneName = null){
        if(bGM1.isPlaying){
            StopSoundBg(bGM1);
            PlaySoundBg(bGM2);
        }
        else{
            StopSoundBg(bGM2);
            PlaySoundBg(bGM1);
        }
        void PlaySoundBg(AudioSource audioSource){
            audioSource.volume = 0;
            audioSource.clip = soundBg.GetSoundBg(audioName, sceneName);
            audioSource.Play();
            audioSource.DOKill();
            audioSource.DOFade(SoundBgVolume, WaitTime.fast)
            .SetEase(Ease.OutQuad);
        }
    }

    /// <summary>
    /// 播放背景音
    /// </summary>
    public void PlaysoundBg(AudioClip audio){
        if(bGM1.isPlaying){
            StopSoundBg(bGM1);
            PlaySoundBg(bGM2);
        }
        else{
            StopSoundBg(bGM2);
            PlaySoundBg(bGM1);
        }
        void PlaySoundBg(AudioSource audioSource){
            audioSource.volume = 0;
            audioSource.clip = audio;
            audioSource.Play();
            audioSource.DOKill();
            audioSource.DOFade(SoundBgOffsetVolume, WaitTime.fast)
            .SetEase(Ease.OutQuad);
        }
    }


    /// <summary>
    /// 停止背景音
    /// </summary>
    public void StopSoundBg(){
        if(bGM1.isPlaying){
            StopSoundBg(bGM1);
        }
        else{
            StopSoundBg(bGM2);
        }
    }

    private void StopSoundBg(AudioSource audioSource){
        audioSource.DOKill();
        audioSource.DOFade(0, WaitTime.fast)
        .SetEase(Ease.OutQuad)
        .OnComplete(StopFinish);
        void StopFinish(){
            audioSource.Stop();
        }
    }
#endregion
#region 音效相关
    /// <summary>
    /// 设置音效音量
    /// </summary>
    public void SetSoundEffectVolume(float volume){
        SoundEffectVolume = volume;
        SFXPool.SetAllSoundEffectVolume(SoundEffectOffsetVolume);
        audioData.UpdateSoundEffectVolume(volume);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlaySoundEffect(string audioName, string sceneName = null){
        var sfx = SFXPool.GetItem();
        sfx.PlaySound(soundEffect.GetSoundEffect(audioName, sceneName));
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlaySoundEffect(AudioClip audio){
        var sfx = SFXPool.GetItem();
        sfx.PlaySound(audio);
    }

    /// <summary>
    /// 停止当前音效
    /// </summary>
    public void StopCurrentSoundEffect(bool isStopiImmediately){
        SFXPool.StopCurrentSound(isStopiImmediately);
    }

    /// <summary>
    /// 停止指定音效
    /// </summary>
    public void StopSpecifySoundEffect(string audioName, string sceneName = null){
        StopSpecifySoundEffect(soundEffect.GetSoundEffect(audioName, sceneName));
    }

    /// <summary>
    /// 停止指定音效
    /// </summary>
    public void StopSpecifySoundEffect(AudioClip audio){
        SFXPool.StopSpecifySoundEffect(audio);
    }

    /// <summary>
    /// 停止所有音效
    /// </summary>
    public void StopAllSoundEffect(){
        SFXPool.StopAllSoundEffect();
    }
#endregion
#region 对话声音相关
    /// <summary>
    /// 设置对话声音音量
    /// </summary>
    public void SetSoundDialogueVolume(float volume){
        SoundEffectVolume = volume;
        VOPool.SetAllSoundEffectVolume(SoundDialogueOffsetVolume);
        audioData.UpdateSoundDialogueVolume(volume);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlaySoundDialogue(string audioName, string sceneName = null){
        var vo = VOPool.GetItem();
        vo.PlaySound(soundDialogue.GetSoundDialogue(audioName, sceneName));
    }

    /// <summary>
    /// 播放对话
    /// </summary>
    public void PlaySoundDialogue(AudioClip audio){
        var vo = VOPool.GetItem();
        vo.PlaySound(audio);
    }

    /// <summary>
    /// 停止当前对话
    /// </summary>
    public void StopCurrentSoundDialogue(bool isStopiImmediately){
        VOPool.StopCurrentSound(isStopiImmediately);
    }

    /// <summary>
    /// 停止所有对话
    /// </summary>
    public void StopAllSoundDialogue(){
        VOPool.StopAllSoundEffect();
    }
#endregion
}
