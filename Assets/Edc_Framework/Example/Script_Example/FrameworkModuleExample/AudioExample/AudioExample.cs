using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioExample : MonoBehaviour
{
    public Button playSoundBtn;
    public Button stopAllSoundBtn;
    public Button playMusic1Btn;
    public Button playMusic2Btn;
    public Button stopMusicBtn;

    private void Start()
    {
        playSoundBtn.onClick.AddListener(ClickPlaySoundBtn);
        stopAllSoundBtn.onClick.AddListener(ClickStopAllSoundBtn);
        playMusic1Btn.onClick.AddListener(ClickPlayMusic1Btn);
        playMusic2Btn.onClick.AddListener(ClickPlayMusic2Btn);
        stopMusicBtn.onClick.AddListener(ClickStopMusicBtn);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    public void ClickPlaySoundBtn(){
        var assetPath = "Assets/Edc_Framework/Example/Sources_Example/SoundEffect_Example/Click.wav";
        var resource = new ResourcePath("Click", assetPath);
        Hub.Audio.PlaySoundEffect(resource);
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
        var assetPath = "Assets/Edc_Framework/Example/Sources_Example/SoundBg_Example/BGM1.mp3";
        var resource = new ResourcePath("BGM1", assetPath);
        Hub.Audio.PlaysoundBg(resource);
    }

    /// <summary>
    /// 播放背景音乐2
    /// </summary>
    public void ClickPlayMusic2Btn(){
        var assetPath = "Assets/Edc_Framework/Example/Sources_Example/SoundBg_Example/BGM2.mp3";
        var resource = new ResourcePath("BGM2", assetPath);
        Hub.Audio.PlaysoundBg(resource);
    }

    /// <summary>
    /// 点击停止背景音
    /// </summary>
    public void ClickStopMusicBtn(){
        Hub.Audio.StopSoundBg();
    }
}
