using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSoundPool<T> : BasePoolManager<T>  where T : BasePool
{
    public AudioSource sound;
    private float playTime;
    protected WaitForSeconds waitForSeconds;
    protected static AudioSource currentSound;
    protected static List<AudioSource> soundList = new List<AudioSource>();

    public override void Create()
    {
        soundList.Add(sound);
    }

    public override void Recycle()
    {
        if(sound == currentSound){
            currentSound = null;
        }
        base.Recycle();
    }

    public virtual void PlaySound(AudioClip audioClip){
        currentSound = sound;
        sound.clip = audioClip;
        sound.Play();
        if(playTime != audioClip.length){
            playTime = audioClip.length;
            waitForSeconds = new WaitForSeconds(playTime);
        }
    }

    /// <summary>
    /// 设置音效音量
    /// </summary>
    public static void SetAllSoundEffectVolume(float volume){
        foreach (var item in soundList)
        {
            item.volume = volume;
        }
    }

    /// <summary>
    /// 停止所有音效
    /// </summary>
    public static void StopAllSoundEffect(){
        foreach (var item in soundList)
        {
            item.Stop();
        }
    }
}
