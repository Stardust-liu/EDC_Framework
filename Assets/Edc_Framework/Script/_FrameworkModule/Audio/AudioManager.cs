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

    private IResourceOwner resourceOwner;
    private AudioClip audioClip;
    
    protected override void Ready()
    {
        base.Ready();
        resourceOwner = Hub.Resources.CreateOwner(GetType().Name);
        SFXPool.InitPool(sFXParent, 5, true).Forget();
        VOPool.InitPool(vOParent, 5, true).Forget();
    }

    protected override void Uninstall()
    {
        base.Uninstall();
        resourceOwner?.ReleaseAll();
        resourceOwner = null;
        audioClip = null;
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
        PlaySoundBgAsync(resourcePath).Forget();
    }

    private async UniTask PlaySoundBgAsync(string resourcePath)
    {
        var audio = await GetOrLoadAudioClip(resourcePath);
        if (audio == null)
        {
            return;
        }
        PlaySoundBg(audio);
    }

    /// <summary>
    /// 播放背景音
    /// </summary>
    public void PlaySoundBg(AudioClip audio){
        if (audio == null)
        {
            return;
        }
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
#region 音频资源加载
    /// <summary>
    /// 加载音频Label
    /// </summary>
    public UniTask LoadLabel(string labelName)
    {
        return resourceOwner.LoadLabel(labelName);
    }

    /// <summary>
    /// 释放音频Label
    /// </summary>
    public void ReleaseLabel(string labelName)
    {
        resourceOwner.ReleaseLabel(labelName);
    }

    private async UniTask<AudioClip> GetOrLoadAudioClip(string resourcePath)
    {
        if (string.IsNullOrEmpty(resourcePath))
        {
            LogManager.LogWarning("音频资源路径为空，无法播放");
            return null;
        }
        if (resourceOwner == null)
        {
            LogManager.LogWarning("AudioManager 尚未初始化资源持有者，无法播放音频");
            return null;
        }

        var audio = (resourceOwner as ResourceOwner)?.TryGetAssetAndLabelAsset<AudioClip>(resourcePath);
        if (audio != null)
        {
            return audio;
        }

        await resourceOwner.LoadAsset(resourcePath);
        return resourceOwner.GetAsset<AudioClip>(resourcePath);
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
    public void PlaySoundEffect(string resourcePath){
        PlaySoundEffectAsync(resourcePath).Forget();
    }

    private async UniTask PlaySoundEffectAsync(string resourcePath)
    {
        var audio = await GetOrLoadAudioClip(resourcePath);
        if (audio == null)
        {
            return;
        }
        await PlaySoundEffectAsync(audio);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public void PlaySoundEffect(AudioClip audio){
        PlaySoundEffectAsync(audio).Forget();
    }

    private async UniTask PlaySoundEffectAsync(AudioClip audio)
    {
        if (audio == null)
        {
            return;
        }
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
        StopSpecifySoundEffect(resourceOwner.GetAsset<AudioClip>(resourcePath));
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
    public void PlaySoundDialogue(string resourcePath){
        PlaySoundDialogueAsync(resourcePath).Forget();
    }

    private async UniTask PlaySoundDialogueAsync(string resourcePath)
    {
        var audio = await GetOrLoadAudioClip(resourcePath);
        if (audio == null)
        {
            return;
        }
        await PlaySoundDialogueAsync(audio);
    }

    /// <summary>
    /// 播放对话
    /// </summary>
    public void PlaySoundDialogue(AudioClip audio){
        PlaySoundDialogueAsync(audio).Forget();
    }

    private async UniTask PlaySoundDialogueAsync(AudioClip audio)
    {
        if (audio == null)
        {
            return;
        }
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
