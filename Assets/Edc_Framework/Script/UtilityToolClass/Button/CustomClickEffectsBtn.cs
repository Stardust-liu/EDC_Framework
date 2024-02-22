using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[Serializable]
public class ContinuousDown : UnityEvent { }
public class CustomClickEffectsBtn : BaseBtn
{
    [ShowIf("isContinuousDown", true)]
    public ContinuousDown continuousDownEvent;

    [ShowIf("isClickZoom", true)]
    public RectTransform zoomTarget;
    [ShowIf("isToggleColor", true)]
    public Image buttonIcon;
    public float tweenTime = 0.15f;
    public bool isClickZoom;
    public bool isToggleColor;

    [LabelText("是否接收持续按下事件")]
    public bool isContinuousDown;
    
    [LabelText("按下几秒算长按")]
    [ShowIf("isContinuousDown", true)]
    public float enterContinuousDownStateTime = 0.5f;

    [LabelText("调用频率（间隔时间）")]
    [ShowIf("isContinuousDown", true)]
    public float performSpeed = 0.2f;

    [LabelText("缩放倍率")]
    [ShowIf("isClickZoom", true)]
    public float zoomRatio = 0.8f;

    [ShowIf("isToggleColor", true)]
    public Color defaultColor = Color.white;

    [ShowIf("isToggleColor", true)]
    public Color onDownColor = Color.white;
    private Vector2 defaultScale;
    private IEnumerator continuous;

    protected override void ButtonInit()
    {
        if (isClickZoom) 
        {
            defaultScale = zoomTarget.localScale;
        }
    }

    protected override void OnDown()
    {
        if (isClickZoom)
        {
            zoomTarget.DOKill();
            zoomTarget.DOScale(new Vector2(defaultScale.x * zoomRatio, defaultScale.y * zoomRatio), tweenTime)
            .SetUpdate(true);
        }
        if (isToggleColor)
        {
            buttonIcon.DOKill();
            buttonIcon.DOColor(onDownColor, tweenTime)
            .SetUpdate(true);
        }
        if (isContinuousDown && continuousDownEvent != null)
        {
            continuous = Continuous();
            StartCoroutine(continuous);
        }
        base.OnDown();
    }

    protected override void OnUp()
    {
        if (isClickZoom)
        {
            zoomTarget.DOKill();
            zoomTarget.DOScale(defaultScale, tweenTime)
            .SetUpdate(true);
        }
        if (isToggleColor)
        {
            buttonIcon.DOKill();
            buttonIcon.DOColor(defaultColor, tweenTime)
            .SetUpdate(true);
        }
        if (isContinuousDown && continuousDownEvent != null)
        {
            StopCoroutine(continuous);
            continuous = null;
        }
        base.OnUp();
    }

    private IEnumerator Continuous()
    {
        yield return new WaitForSecondsRealtime(enterContinuousDownStateTime);
        var waitPerform = new WaitForSecondsRealtime(performSpeed);
        while (true)
        {
            yield return waitPerform;
            continuousDownEvent.Invoke();
        }
    }
}
