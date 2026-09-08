using System;
using UnityEngine;

public class AnimatorUITransition : BaseUITransition
{
    [SerializeField]
    private Animator animator;
    private readonly string showTrigger = "Show";
    private readonly string hideTrigger = "Hide";
    private Action showComplete;
    private Action hideComplete;

    public override void PlayShow(Action onComplete)
    {
        showComplete = onComplete;
        if (!CanPlay(showTrigger))
        {
            ShowFinish();
            return;
        }
        animator.ResetTrigger(hideTrigger);
        animator.SetTrigger(showTrigger);
    }

    public override void PlayHide(Action onComplete)
    {
        hideComplete = onComplete;
        if (!CanPlay(hideTrigger))
        {
            HideFinish();
            return;
        }
        animator.ResetTrigger(showTrigger);
        animator.SetTrigger(hideTrigger);
    }

    public void ShowFinish()
    {
        var complete = showComplete;
        showComplete = null;
        complete?.Invoke();
    }

    public void HideFinish()
    {
        var complete = hideComplete;
        hideComplete = null;
        complete?.Invoke();
    }

    private bool CanPlay(string triggerName)
    {
        return animator != null
               && animator.runtimeAnimatorController != null
               && !string.IsNullOrEmpty(triggerName);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        TryGetComponent(out animator);
    }
#endif
}
