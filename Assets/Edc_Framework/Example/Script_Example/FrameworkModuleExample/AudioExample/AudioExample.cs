using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AudioExample : MonoBehaviour
{
    public Button playSoundBtn;
    public Button stopAllSoundBtn;
    public Button playMusic1Btn;
    public Button playMusic2Btn;
    public Button stopMusicBtn;
    public LabelManager soundManager;

    private void Start()
    {
        LabelManager.Init(out soundManager, "Example_Audio").LoadLabel().Forget();

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
        Hub.Audio.PlaySoundEffect("Click_Example");
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
