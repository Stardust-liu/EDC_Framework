using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioExample : MonoBehaviour
{
    public CustomClickEffectsBtn playSoundBtn;
    public CustomClickEffectsBtn stopAllSoundBtn;
    public CustomClickEffectsBtn playMusic1Btn;
    public CustomClickEffectsBtn playMusic2Btn;
    public CustomClickEffectsBtn stopMusicBtn;

    private void Start() {
        playSoundBtn.onClickEvent.AddListener(ClickPlaySoundBtn);
        stopAllSoundBtn.onClickEvent.AddListener(ClickStopAllSoundBtn);
        playMusic1Btn.onClickEvent.AddListener(ClickPlayMusic1Btn);
        playMusic2Btn.onClickEvent.AddListener(ClickPlayMusic2Btn);
        stopMusicBtn.onClickEvent.AddListener(ClickStopMusicBtn);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public void ClickPlaySoundBtn(){
        Hub.Audio.PlaySoundEffect("Click");
    }

    /// <summary>
    /// 停止音效
    /// </summary>
    public void ClickStopAllSoundBtn(){
        Hub.Audio.StopAllSoundEffect();
    }

    /// <summary>
    /// 播放背景音乐1
    /// </summary>
    public void ClickPlayMusic1Btn(){
        Hub.Audio.PlaysoundBg("BGM1");
    }

    /// <summary>
    /// 播放背景音乐2
    /// </summary>
    public void ClickPlayMusic2Btn(){
        Hub.Audio.PlaysoundBg("BGM2");
    }

    /// <summary>
    /// 点击停止背景音
    /// </summary>
    public void ClickStopMusicBtn(){
        Hub.Audio.StopSoundBg();
    }
}
