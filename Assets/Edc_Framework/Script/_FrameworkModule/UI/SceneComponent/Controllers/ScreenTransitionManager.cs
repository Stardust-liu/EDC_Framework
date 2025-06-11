using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScreenTransitionManager : BaseMonoIOCComponent
{
    public TweenColor fadeTween;
    private static Color transparentColor = new Color(1, 1, 1, 0);

    /// <summary>
    /// 淡入
    /// </summary>
    public Tweener FadeIn(Color color, float fadeInTime){
        fadeTween.startColor = color * transparentColor;
        fadeTween.targetColor = color;
        fadeTween.SetDuration(fadeInTime);
        fadeTween.SetToStart();
        return fadeTween.Play();
    }

    /// <summary>
    /// 淡出
    /// </summary>
    public Tweener FadeOut(float fadeInTime = -1){
        fadeTween.SetDuration(fadeInTime);
        return fadeTween.Play(false);
    }
}
