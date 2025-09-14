using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionItemSliderInfo
{
    public UnityAction<float> valueChange;
    public float initValue;

    public OptionItemSliderInfo(UnityAction<float> _valueChange, float _initValue)
    {
        valueChange = _valueChange;
        initValue = _initValue;
    }
}

public class OptionItemSlider : BaseOptionItem
{
    public Slider slider;
    [FoldoutGroup("切换Value音效")]
    public string fileNmae_AB;
    [FoldoutGroup("切换Value音效")]
    public string assetsPath;
    private float lastPlaySoundTime;
    private UnityAction<float> valueChange;

    public void SetInitAction(OptionItemSliderInfo optionItemSliderInfo, Action<Transform, string[]> _changeOptionItem, Action _exitBtn)
    {
        slider.value = optionItemSliderInfo.initValue;
        valueChange = optionItemSliderInfo.valueChange;
        slider.onValueChanged.AddListener(ValueChange);
        SetInitAction(_changeOptionItem, _exitBtn);
    }

    private void ValueChange(float value)
    {
        PlaySound();
        valueChange.Invoke(value);
    }

    private void PlaySound()
    {
        if (lastPlaySoundTime == 0 || Time.time - lastPlaySoundTime > 0.1f)
        {
            Hub.Audio.PlaySoundEffect(new ResourcePath(fileNmae_AB, assetsPath));
            lastPlaySoundTime = Time.time;
        }
    }
}
