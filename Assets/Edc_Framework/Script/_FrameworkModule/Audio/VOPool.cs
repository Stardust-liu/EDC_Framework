using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class VOPool : BaseSoundPool<VOPool>
{
    public override void Create()
    {
        if(Hub.Audio != null){
            sound.volume = Hub.Audio.SoundDialogueOffsetVolume;
        }
        base.Create();
    }

    public override void PlaySound(AudioClip audioClip){
        base.PlaySound(audioClip);
        StartCoroutine(WaitPlayComplete());
    }

    /// <summary>
    /// 停止当前对话声音
    /// </summary>
    public static void StopCurrentSound(bool isStopiImmediately){
        if(currentSound != null){
            if(isStopiImmediately){
                currentSound.Stop();
            }
            else{
                var temporarySound = currentSound;
                temporarySound.DOKill();
                temporarySound.DOFade(0, WaitTime.fast).OnComplete(StopFinish);
                void StopFinish(){
                    temporarySound.Stop();
                    temporarySound.volume = Hub.Audio.SoundDialogueOffsetVolume;
                }
            }
        }
    }

    private IEnumerator WaitPlayComplete(){
        yield return waitForSeconds;
        RecycleItem(this);
    }
}
