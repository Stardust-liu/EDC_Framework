using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class TweenGroup : MonoBehaviour
{
    [OnValueChanged(nameof(SetTweensCount))]
    public BaseTween[] baseTweens;

    [LabelText("是否取反控制")]
    public bool IsInvertedForwardPlay;

    [SerializeField, HideInInspector]
    private int tweensCount;
    private int completeTweenCount;
    private Action OnAllTweenComplete;

    [Button("SetAllTweenToStart")]
    public void SetAllWtwweToStart()
    {
        SetAllTweenToStart();
    }

    [Button("SetAllTweenToTarget")]
    public void SetAllWtwweToTarget()
    {
        SetAllTweenToTarget();
    }

    private void SetTweensCount()
    {
        tweensCount = baseTweens == null ? 0 : baseTweens.Length;
    }

    /// <summary>
    /// 播放动画
    /// </summary>
    public void Play(bool isForwardPlay = true)
    {
        if (IsInvertedForwardPlay)
        {
            isForwardPlay = !isForwardPlay;
        }
        completeTweenCount = 0;
        SetTweensCount();
        if (tweensCount <= 0)
        {
            CompleteAllTween();
            return;
        }
        for (var i = 0; i < tweensCount; i++)
        {
            var baseTween = baseTweens[i];
            if (baseTween == null)
            {
                OnComplete();
                continue;
            }
            var tweener = baseTween.Play(isForwardPlay);
            if (tweener == null)
            {
                OnComplete();
                continue;
            }
            tweener.OnComplete(OnComplete);
        }
    }

    /// <summary>
    /// 暂停动画
    /// </summary>
    public void Pause()
    {
        if (baseTweens == null)
        {
            return;
        }
        for (var i = 0; i < baseTweens.Length; i++)
        {
            baseTweens[i]?.Pause();
        }
    }

    /// <summary>
    /// 添加动画播放结束事件
    /// </summary>
    public void AddListenerTween(Action callBack)
    {
        OnAllTweenComplete += callBack;
    }

    /// <summary>
    /// 清空动画播放结束事件
    /// </summary>
    public void ClearAllTweenCompleteListener()
    {
        OnAllTweenComplete = null;
    }

    /// <summary>
    /// 设置所有动画到起点位置
    /// </summary>
    public void SetAllTweenToStart()
    {
        if (baseTweens == null)
        {
            return;
        }
        for (var i = 0; i < baseTweens.Length; i++)
        {
            baseTweens[i]?.SetToStart();
        }
    }

    /// <summary>
    /// 设置所有动画到终点位置
    /// </summary>
    public void SetAllTweenToTarget()
    {
        if (baseTweens == null)
        {
            return;
        }
        for (var i = 0; i < baseTweens.Length; i++)
        {
            baseTweens[i]?.SetToTarget();
        }
    }

    private void OnComplete()
    {
        if (++completeTweenCount >= tweensCount)
        {
            CompleteAllTween();
        }
    }

    private void CompleteAllTween()
    {
        var complete = OnAllTweenComplete;
        OnAllTweenComplete = null;
        complete?.Invoke();
    }
}
