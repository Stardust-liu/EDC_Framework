using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingController : BaseMonoIOCComponent
{
    [SerializeField]
    private RectTransform thisRectTransform;

    [SerializeField]
    private Image LoadingBG;

    [SerializeField]
    private Image LoadingSlider;

    [SerializeField]
    private TextMeshProUGUI LoadingValue;

    private Action loadingFinishCallBack;

    private float value = -1;

    /// <summary>
    /// 显示加载面板
    /// </summary>
    public void ShowLoadingPanel(){
        thisRectTransform.anchoredPosition = Vector2.zero;
    }

    /// <summary>
    /// 设置加载背景
    /// </summary>
    public void SetLoadingBG(Sprite bg){
        LoadingBG.sprite = bg;
    }

    /// <summary>
    /// 设置加载进度
    /// </summary>
    public void SetLoadSchedule(float _value){
        if(value != _value){
            value = _value;
            LoadingSlider.fillAmount = value;
            LoadingValue.text = $"{_value}%";
            thisRectTransform.anchoredPosition = Vector2.one*10000;
            if(loadingFinishCallBack != null){
                loadingFinishCallBack.Invoke();
                loadingFinishCallBack = null;
            }
        }
    }

    /// <summary>
    /// 设置加载完成回调
    /// </summary>
    public void SetLoadingFinishCallBack(Action callback){
        loadingFinishCallBack = callback;
    }
}
