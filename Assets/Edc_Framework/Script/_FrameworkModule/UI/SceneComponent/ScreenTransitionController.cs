using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ScreenTransitionController : MonoBehaviour
{
    public TweenColor fadeTween;
    private static Color transparentColor = new Color(1, 1, 1, 0);

    /// <summary>
    /// 淡入
    /// </summary>
    public void FadeIn(Color color){
        gameObject.layer = LayerMask.NameToLayer("UI");
        fadeTween.startColor = color;
        fadeTween.targetColor = color * transparentColor;
        fadeTween.SetToStart();
        fadeTween.Play();
    }

    /// <summary>
    /// 淡出
    /// </summary>
    public void FadeOut(){
        fadeTween.Play(false).OnComplete(FadeOutFinish);
    }

    private void FadeOutFinish(){
        gameObject.layer = LayerMask.NameToLayer("UI_Hide");
    }
}
