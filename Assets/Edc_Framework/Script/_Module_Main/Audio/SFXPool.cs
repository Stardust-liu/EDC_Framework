using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class SFXPool : BaseSoundPool<SFXPool>
{
    public override void Create()
    {
        sound.volume = Hub.Audio.SoundEffectOffsetVolume;
        base.Create();
    }

    public override void PlaySound(AudioClip audioClip){
        base.PlaySound(audioClip);
        StartCoroutine(WaitPlayComplete());
    }

    /// <summary>
    /// 停止指定音效
    /// </summary>
    public static void StopSpecifySoundEffect(AudioClip audio){
        foreach (var item in soundList)
        {
            if(item.isPlaying && item.clip == audio){
                item.Stop();
            }
        }
    }

    /// <summary>
    /// 停止当前音效
    /// </summary>
    public static void StopCurrentSound(bool isStopiImmediately){
        if(currentSound != null){
            if(isStopiImmediately){
                currentSound.Stop();
                currentSound = null;
            }
            else{
                var temporarySound = currentSound;
                temporarySound.DOKill();
                temporarySound.DOFade(0, WaitTime.fast).OnComplete(StopFinish);
                void StopFinish(){
                    temporarySound.Stop();
                    temporarySound.volume = Hub.Audio.SoundEffectOffsetVolume;
                }
            }
        }
    }

    private IEnumerator WaitPlayComplete(){
        yield return waitForSeconds;
        RecycleItem(this);
    }
}
