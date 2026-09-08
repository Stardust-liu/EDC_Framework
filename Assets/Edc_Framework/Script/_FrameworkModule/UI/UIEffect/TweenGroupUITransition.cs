using System;
using UnityEngine;

public class TweenGroupUITransition : BaseUITransition
{
    [SerializeField]
    private TweenGroup showTween;
    [SerializeField]
    private TweenGroup hideTween;
    [SerializeField]
    private bool showForward = true;
    [SerializeField]
    private bool hideForward = true;
    [SerializeField]
    private bool setShowTweenToStart = true;
    [SerializeField]
    private bool setHideTweenToStart = true;

    public override void PlayShow(Action onComplete)
    {
        PlayTween(showTween, showForward, setShowTweenToStart, onComplete);
    }

    public override void PlayHide(Action onComplete)
    {
        PlayTween(hideTween, hideForward, setHideTweenToStart, onComplete);
    }

    private static void PlayTween(TweenGroup tweenGroup, bool isForwardPlay, bool setToStart, Action onComplete)
    {
        if (tweenGroup == null)
        {
            onComplete?.Invoke();
            return;
        }
        tweenGroup.ClearAllTweenCompleteListener();
        tweenGroup.AddListenerTween(onComplete);
        if (setToStart)
        {
            tweenGroup.SetAllTweenToStart();
        }
        tweenGroup.Play(isForwardPlay);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        AutoSetTweenGroups();
    }

    [ContextMenu("Auto Set Tween Groups")]
    private void AutoSetTweenGroups()
    {
        var tweenGroups = GetComponentsInChildren<TweenGroup>(true);
        foreach (var tweenGroup in tweenGroups)
        {
            if (showTween == null && CheckName(tweenGroup, "TweenIn", "Show"))
            {
                showTween = tweenGroup;
            }
            if (hideTween == null && CheckName(tweenGroup, "TweenOut", "Hide"))
            {
                hideTween = tweenGroup;
            }
        }
    }

    private static bool CheckName(Component component, string firstName, string secondName)
    {
        return component.name.IndexOf(firstName, StringComparison.OrdinalIgnoreCase) >= 0
               || component.name.IndexOf(secondName, StringComparison.OrdinalIgnoreCase) >= 0;
    }
#endif
}
