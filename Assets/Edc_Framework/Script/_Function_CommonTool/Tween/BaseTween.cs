using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

[InlineEditor]
public abstract class BaseTween : MonoBehaviour
{
    [LabelText("Tween时间"), SerializeField]
    protected float duration = WaitTime.fast;
    [LabelText("延迟时间"), SerializeField]
    protected float delay = 0;

    public Ease ease = Ease.OutQuad;
    public bool setUpdate;
    private Tweener tweener;

    public Tweener Play(bool isForwardPlay = true)
    {
        Kill();
        tweener = isForwardPlay ? ForwardPlay() : ReversePlay();
        tweener?.SetLink(gameObject);
        return tweener;
    }

    public void Pause()
    {
        tweener?.Pause();
    }

    public void Kill()
    {
        if (tweener != null && tweener.IsActive())
        {
            tweener.Kill();
        }
        tweener = null;
    }

    protected abstract Tweener ForwardPlay();
    protected abstract Tweener ReversePlay();

    [Button("SetToStart")]
    public abstract void SetToStart();
    [Button("SetToTarget")]
    public abstract void SetToTarget();
    [Button("SwapStartAndTarget")]
    public abstract void SwapStartAndTarget();

    public void SetDuration(float _duration)
    {
        duration = _duration;
    }

    public void SetDelay(float _delay)
    {
        delay = _delay;
    }

    public float GetDuration()
    {
        return duration;
    }

    public float GetDelay()
    {
        return delay;
    }
}
