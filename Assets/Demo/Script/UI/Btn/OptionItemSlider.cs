using System;
using System.Collections;
using System.Collections.Generic;
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
    public void SetInitAction(OptionItemSliderInfo optionItemSliderInfo, Action<Transform, string[]> _changeOptionItem, Action _exitBtn)
    {
        slider.value = optionItemSliderInfo.initValue;
        slider.onValueChanged.AddListener(optionItemSliderInfo.valueChange);
        SetInitAction(_changeOptionItem, _exitBtn);
    }
}
