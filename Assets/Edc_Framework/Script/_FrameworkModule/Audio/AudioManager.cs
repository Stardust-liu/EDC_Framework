using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ArchiveData;
using Cysharp.Threading.Tasks;

public enum AudioType{
    SoundBg,//背景音
    SoundEffect,//音效
    SoundDialogue,//对话声音
}

public class AudioManager : BaseMonoIOCComponent<AudioData>
{
    [SerializeField]
    private AudioSource bGM1;
    [SerializeField]
    private AudioSource bGM2;
    [SerializeField]
    private Transform sFXParent;
    [SerializeField]
    private Transform vOParent;

    public float SoundMainVolume{get {return Data.soundMainVolume;}}
    public float SoundBgVolume{get {return Data.soundBgVolume;}}
    public float SoundEffectVolume{get {return Data.soundEffectVolume;}}
    public float SoundDialogueVolume{get {return Data.soundDialogueVolume;}}

    public float SoundBgOffsetVolume {get{return SoundBgVolume * SoundMainVolume;}}
    public float SoundEffectOffsetVolume {get{return SoundEffectVolume * SoundMainVolume;}}
    public float SoundDialogueOffsetVolume {get{return SoundDialogueVolume * SoundMainVolume;}}

    private ResourcesModule resourcesModule;
    private AudioClip audioClip;
    
    protected override void Init() {
        base.Init();
        resourcesModule = Hub.Resources;
        SFXPool.InitPool(sFXParent, 5, true).Forget();
        VOPool.InitPool(vOParent, 5, true).Forget();
    }

#region 主音音量相关
    /// <summary>
    /// 设置主音量
    /// </summary>
    public void SetSoundMainVolume(float volume){
        Data.UpdtaeSoundMainVolume(volume);
        bGM1.volume = bGM2.volume = SoundBgOffsetVolume;
        SFXPool.SetAllSoundEffectVolume(SoundEffectOffsetVolume);
        VOPool.SetAllSoundEffectVolume(SoundDialogueOffsetVolume);
    }
#endregion
#region 背景音相关
    /// <summary>
    /// 设置背景音乐音量
    /// </summary>
    public void SetSoundBgVolume(float volume){
        Data.UpdateSoundBgVolume(volume);
        bGM1.volume = SoundBgOffsetVolume;
        bGM2.volume = SoundBgOffsetVolume;
    }


    /// <summary>
    /// 播放背景音
    /// </summary>
    public void PlaysoundBg(string resourcePath){
        PlaySoundBg(resourcesModule.Get<AudioClip>(resourcePath));
    }

    /// <summary>
    /// 播放背景音
    /// </summary>
    public void PlaySoundBg(AudioClip audio){
        if (audioClip != null && audioClip == audio)
        {
            return;
        }
        else
        {
            audioClip = audio;
        }
        if (bGM1.isPlaying)
        {
            StopSoundBg(bGM1);
            PlaySoundBg(bGM2);
        }
        else
        {
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
        Data.UpdateSoundEffectVolume(volume);
        SFXPool.SetAllSoundEffectVolume(SoundEffectOffsetVolume);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public async void PlaySoundEffect(string resourcePath){
        var sfx = await SFXPool.GetItem();
        sfx.PlaySound(resourcesModule.Get<AudioClip>(resourcePath));
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public async void PlaySoundEffect(AudioClip audio){
        var sfx = await SFXPool.GetItem();
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
    public void StopSpecifySoundEffect(string resourcePath){
        StopSpecifySoundEffect(resourcesModule.Get<AudioClip>(resourcePath));
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
        Data.UpdateSoundDialogueVolume(volume);
        VOPool.SetAllSoundEffectVolume(SoundDialogueOffsetVolume);
    }

    /// <summary>
    /// 播放对话
    /// </summary>
    public async void PlaySoundDialogue(string resourcePath){
        var vo = await VOPool.GetItem();
        vo.PlaySound(resourcesModule.Get<AudioClip>(resourcePath));
    }

    /// <summary>
    /// 播放对话
    /// </summary>
    public async void PlaySoundDialogue(AudioClip audio){
        var vo = await VOPool.GetItem();
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
