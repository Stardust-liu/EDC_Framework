using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;


public class TweenGroup : MonoBehaviour
{
    public delegate void AllTweenComplete();
    public event AllTweenComplete OnAllTweenComplete;
    [OnValueChanged(nameof(SetTweensCount))]
    public BaseTween[] baseTweens;
    [SerializeField, HideInInspector]
    private int tweensCount;
    private int completeTweenCount;

    private void SetTweensCount(){
        tweensCount = baseTweens.Length;
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    public void Play(bool isForwardPlay = true){
        completeTweenCount = 0;
        for (var i = 0; i < tweensCount; i++)
        {
            baseTweens[i].Play(isForwardPlay).OnComplete(OnComplete);
        }
    }

    /// <summary>
    /// 暂停动画
    /// </summary>
    public void Pause(){
        for (var i = 0; i < tweensCount; i++)
        {
            baseTweens[i].Pause();
        }
    }

    /// <summary>
    /// 清空动画播放结束事件
    /// </summary>
    public void ClearAllTweenCompleteListener(){
        OnAllTweenComplete = null;
    }

    /// <summary>
    /// 设置所有动画到起点位置
    /// </summary>
    public void SetAllTweenToStart(){
        for (var i = 0; i < tweensCount; i++)
        {
            baseTweens[i].SetToStart();
        }
    }

    /// <summary>
    /// 设置所有动画到终点位置
    /// </summary>
    public void SetAllTweenToTarget(){
        for (var i = 0; i < tweensCount; i++)
        {
            baseTweens[i].SetToTarget();
        }
    }

    private void OnComplete(){
        if(++completeTweenCount == tweensCount){
            OnAllTweenComplete?.Invoke();
        }
    }
}
