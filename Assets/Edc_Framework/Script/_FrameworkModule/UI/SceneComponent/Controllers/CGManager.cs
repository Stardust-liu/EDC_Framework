using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class CGManager : BaseMonoIOCComponent
{
    public Image bg;
    public Image Image_CG;
    public VideoPlayer Video_CG;

    private Action videoPlayEndCallBack;

    public bool IsVideoPlaying => Video_CG != null && Video_CG.isPlaying;
    public double VideoTime => Video_CG != null ? Video_CG.time : 0d;
    public double VideoLength => Video_CG != null && Video_CG.clip != null ? Video_CG.clip.length : 0d;

    protected override void Init()
    {
        if (Video_CG == null)
        {
            LogManager.LogError("CGManager missing VideoPlayer");
            return;
        }

        Video_CG.playOnAwake = false;
        Video_CG.loopPointReached -= OnVideoPlayEnd;
        Video_CG.loopPointReached += OnVideoPlayEnd;
    }

    public void ShowImage(Sprite sprite)
    {
        StopVideo();
        gameObject.SetActive(true);

        if (bg != null)
        {
            bg.enabled = true;
        }

        if (Image_CG == null)
        {
            LogManager.LogError("CGManager missing CG Image");
            return;
        }

        Image_CG.enabled = true;
        Image_CG.sprite = sprite;
    }

    public void ShowVido(VideoClip videoClip, Action _vidoPlayEndCallBack = null)
    {
        ShowVideo(videoClip, _vidoPlayEndCallBack);
    }

    public void ShowVideo(VideoClip videoClip, Action videoPlayEndCallBack = null)
    {
        if (Video_CG == null)
        {
            LogManager.LogError("CGManager missing VideoPlayer");
            return;
        }

        if (videoClip == null)
        {
            LogManager.LogError("CGManager received empty VideoClip");
            return;
        }

        gameObject.SetActive(true);

        if (bg != null)
        {
            bg.enabled = true;
        }

        if (Image_CG != null)
        {
            Image_CG.enabled = false;
        }

        this.videoPlayEndCallBack = videoPlayEndCallBack;
        Video_CG.Stop();
        Video_CG.clip = videoClip;
        Video_CG.time = 0d;
        ApplyVideoVolume();
        Video_CG.Play();
    }

    public void PauseVideo()
    {
        if (Video_CG == null)
        {
            return;
        }

        Video_CG.Pause();
    }

    public void ResumeVideo()
    {
        if (Video_CG == null || Video_CG.clip == null)
        {
            return;
        }

        Video_CG.Play();
    }

    public void StopVideo()
    {
        if (Video_CG == null)
        {
            return;
        }

        videoPlayEndCallBack = null;
        Video_CG.Stop();
    }

    public void SeekVideo(double time)
    {
        if (Video_CG == null || !Video_CG.canSetTime)
        {
            return;
        }

        Video_CG.time = Math.Max(0d, time);
    }

    public void HideCgCanvas()
    {
        StopVideo();
        gameObject.SetActive(false);
    }

    protected override void Uninstall()
    {
        base.Uninstall();

        if (Video_CG != null)
        {
            Video_CG.loopPointReached -= OnVideoPlayEnd;
        }

        videoPlayEndCallBack = null;
    }

    private void OnVideoPlayEnd(VideoPlayer videoPlayer)
    {
        var callback = videoPlayEndCallBack;
        videoPlayEndCallBack = null;
        callback?.Invoke();
    }

    private void ApplyVideoVolume()
    {
        if (Hub.Audio == null)
        {
            return;
        }
        Video_CG.SetDirectAudioVolume(0, Hub.Audio.SoundMainVolume);
    }
}
