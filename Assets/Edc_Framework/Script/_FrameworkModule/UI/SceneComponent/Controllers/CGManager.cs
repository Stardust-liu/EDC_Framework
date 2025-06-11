using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CGManager : BaseMonoIOCComponent
{
    public Image bg;
    public Image Image_CG;
    public VideoPlayer Video_CG;
    private Action vidoPlayEndCallBack;

    protected override void Init(){
        Video_CG.loopPointReached += VidoPlayEnd;
    }

    /// <summary>
    /// 显示CG图片
    /// </summary>
    public void ShowImage(Sprite sprite){
        gameObject.layer = LayerMask.NameToLayer("UI");
        bg.enabled = true;
        Image_CG.sprite = sprite;
    }

    /// <summary>
    /// 显示CG视频
    /// </summary>
    public void ShowVido(VideoClip videoClip, Action _vidoPlayEndCallBack = null){
        gameObject.layer = LayerMask.NameToLayer("UI");
        vidoPlayEndCallBack = _vidoPlayEndCallBack;
        Video_CG.SetDirectAudioVolume(0, Hub.Audio.SoundMainVolume);
        Video_CG.clip = videoClip;
        Video_CG.Play();
    }

    /// <summary>
    /// 隐藏Cg面板
    /// </summary>
    public void HideCgCanvas(){
        gameObject.layer = LayerMask.NameToLayer("UI_Hide");
    }

    private void VidoPlayEnd(VideoPlayer vp){
        if(vidoPlayEndCallBack != null){
            vidoPlayEndCallBack.Invoke();
            vidoPlayEndCallBack = null;
        }
    }
}